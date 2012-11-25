using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMapper.conventions;
using POCOMapper.exceptions;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.standard;

namespace POCOMapper.definition
{
	/// <summary>
	/// One defined and compiled mappings set.
	/// </summary>
	public class MappingImplementation
	{
		private readonly List<IMappingDefinition> aMappingDefinitions;
		private readonly Dictionary<Tuple<Type, Type>, IMappingDefinition> aContainerMappingDefinitions;
		private readonly Dictionary<Tuple<Type, Type>, IMapping> aMappings;
		private List<IChildAssociationPostprocessing> aChildPostprocessings;

		internal MappingImplementation(IEnumerable<IMappingDefinition> mappingDefinitions, IEnumerable<IChildAssociationPostprocessing> childPostprocessings, Conventions fromConventions, Conventions toConventions)
		{
			this.aMappings = new Dictionary<Tuple<Type, Type>, IMapping>();

			this.aMappingDefinitions = mappingDefinitions.OrderBy(x => x.Priority).ToList();

			this.aChildPostprocessings = childPostprocessings.ToList();

			this.FromConventions = fromConventions;
			this.ToConventions = toConventions;
		}

		internal Conventions FromConventions { get; private set; }
		internal Conventions ToConventions { get; private set; }

		/// <summary>
		/// Finds the mapping from the type specified by the <paramref name="from"/> parameter to the type specified
		/// by the <paramref name="to"/> parameter.
		/// </summary>
		/// <param name="from">Class from the source model.</param>
		/// <param name="to">Class from the destination model.</param>
		/// <returns>The mapping specified by the parameters.</returns>
		public IMapping GetMapping(Type from, Type to)
		{
			Tuple<Type, Type> key = new Tuple<Type, Type>(from, to);

			if (this.aMappings.ContainsKey(key))
				return this.aMappings[key];

			foreach (IMappingDefinition mappingDefinition in aMappingDefinitions)
			{
				if (mappingDefinition.IsFrom(from) && mappingDefinition.IsTo(to))
				{
					IMapping mapping = mappingDefinition.CreateMapping(this, from, to);
					this.aMappings[key] = mapping;
					return mapping;
				}
			}

			if (from == to)
				return (IMapping) Activator.CreateInstance(typeof(Copy<>).MakeGenericType(from), this);

			return null;
		}

		/// <summary>
		/// Type-safe version of the GetMapping method. Finds the mapping from the type specified by
		/// the <typeparamref name="TFrom"/> parameter to the type specified by the <typeparamref name="TTo"/> parameter.
		/// </summary>
		/// <typeparam name="TFrom">Class from the source model.</typeparam>
		/// <typeparam name="TTo">Class from the destination model.</typeparam>
		/// <returns>The mapping specified by the type parameters.</returns>
		public IMapping<TFrom, TTo> GetMapping<TFrom, TTo>()
		{
			return (IMapping<TFrom, TTo>)GetMapping(typeof(TFrom), typeof(TTo));
		}

		/// <summary>
		/// Map the instance of the class from the source model onto the new instance of the class from the destination model.
		/// </summary>
		/// <typeparam name="TFrom">Class from the source model.</typeparam>
		/// <typeparam name="TTo">Class from the destination model.</typeparam>
		/// <param name="from">Instance that should be mapped.</param>
		/// <returns>New mapped instance.</returns>
		public TTo Map<TFrom, TTo>(TFrom from)
		{
			IMapping<TFrom, TTo> mapping = this.GetMapping<TFrom, TTo>();

			if (mapping == null)
				throw new UnknownMapping(typeof(TFrom), typeof(TTo));

			if (!mapping.CanMap)
				throw new CantMap(string.Format("Can't map {0} to {1}, mapping object does not support simple mapping", typeof(TFrom).Name, typeof(TTo).Name));

			return mapping.Map(from);
		}

		/// <summary>
		/// Transfer state of the instance specified by the <paramref name="from"/> parameter to the instance specified
		/// by the <paramref name="to"/> parameter.
		/// </summary>
		/// <typeparam name="TFrom">Class from the source model.</typeparam>
		/// <typeparam name="TTo">Class from the destination model.</typeparam>
		/// <param name="from">Instance that should be mapped.</param>
		/// <param name="to">Instance that should have state transfered to.</param>
		public void Synchronize<TFrom, TTo>(TFrom from, TTo to)
		{
			IMapping<TFrom, TTo> mapping = this.GetMapping<TFrom, TTo>();

			if (mapping == null)
				throw new UnknownMapping(typeof(TFrom), typeof(TTo));

			if (!mapping.CanSynchronize)
				throw new CantMap(string.Format("Can't synchronize {0} to {1}, mapping object does not support synchronization", typeof(TFrom).Name, typeof(TTo).Name));

			mapping.Synchronize(from, to);
		}

		private void MappingToString(IMapping mapping, StringBuilder output, int level, List<IMapping> allMappings = null)
		{
			string indent = string.Concat(Enumerable.Range(0, level).Select(x => "    "));

			if (mapping == null)
			{
				output.Append("(null)\n");
			}
			else
			{
				Type mappingType = mapping.GetType();
				output.Append(mappingType.Name);

				if (mappingType.IsGenericType)
				{
					bool begining = true;
					output.Append("<");

					foreach (Type genericArgument in mappingType.GetGenericArguments())
					{
						if (!begining)
							output.Append(", ");
						output.Append(genericArgument.Name);
						begining = false;
					}

					output.Append(">");
				}

				output.Append("\n");

				foreach (Tuple<string, IMapping> child in mapping.Children)
				{
					output.Append(indent);
					output.Append(child.Item1);
					output.Append(" ");

					if (allMappings != null)
						allMappings.Add(child.Item2);

					this.MappingToString(child.Item2, output, level + 1, allMappings);
				}
			}
		}

		/// <summary>
		/// Builds the debug string of the mapping given mapping.
		/// </summary>
		/// <typeparam name="TFrom">Class from the source model.</typeparam>
		/// <typeparam name="TTo">Class from the destination model.</typeparam>
		/// <returns></returns>
		public string MappingToString<TFrom, TTo>()
		{
			StringBuilder output = new StringBuilder();

			this.MappingToString(this.GetMapping<TFrom, TTo>(), output, 1);

			return output.ToString();
		}

		///<summary>
		/// Buields the debug string for all the mappings
		///</summary>
		///<returns></returns>
		public string AllMappingsToString()
		{
			List<Tuple<IMapping, StringBuilder>> results = new List<Tuple<IMapping, StringBuilder>>();
			List<IMapping> toIgnore = new List<IMapping>();

			foreach (IMappingDefinition mappingDefinition in this.aMappingDefinitions)
			{
				Tuple<Type, Type> key = mappingDefinition.GetKey();

				if (key != null)
				{
					IMapping mapping = this.GetMapping(key.Item1, key.Item2);

					StringBuilder output = new StringBuilder();
					this.MappingToString(mapping, output, 1, toIgnore);
					results.Add(new Tuple<IMapping, StringBuilder>(mapping, output));
				}
			}

			StringBuilder ret = new StringBuilder();

			foreach (Tuple<IMapping, StringBuilder> result in results)
			{
				if (!toIgnore.Contains(result.Item1))
				{
					ret.Append(result.Item2.ToString());
					ret.Append("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
				}
			}

			return ret.ToString();
		}

		/// <summary>
		/// Returns source for compiled mapping.
		/// </summary>
		/// <typeparam name="TFrom">Class from the source model.</typeparam>
		/// <typeparam name="TTo">Class from the destination model.</typeparam>
		/// <returns>Source code, or null if no source code is generated by mapping</returns>
		public string GetMappingSource<TFrom, TTo>()
		{
			return this.GetMappingSource(typeof(TFrom), typeof(TTo));
		}

		/// <summary>
		/// Returns source for compiled mapping.
		/// </summary>
		/// <param name="from">Class from the source model.</param>
		/// <param name="to">Class from the destination model.</param>
		/// <returns>Source code, or null if no source code is generated by mapping</returns>
		public string GetMappingSource(Type from, Type to)
		{
			return this.GetMapping(from, to).MappingSource;
		}

		/// <summary>
		/// Returns source for compiled synchronization.
		/// </summary>
		/// <typeparam name="TFrom">Class from the source model.</typeparam>
		/// <typeparam name="TTo">Class from the destination model.</typeparam>
		/// <returns>Source code, or null if no source code is generated by mapping</returns>
		public string GetSynchronizationSource<TFrom, TTo>()
		{
			return this.GetSynchronizationSource(typeof(TFrom), typeof(TTo));
		}

		/// <summary>
		/// Returns source for compiled synchronization.
		/// </summary>
		/// <param name="from">Class from the source model.</param>
		/// <param name="to">Class from the destination model.</param>
		/// <returns>Source code, or null if no source code is generated by mapping</returns>
		public string GetSynchronizationSource(Type from, Type to)
		{
			return this.GetMapping(from, to).SynchronizationSource;
		}

		internal Delegate GetChildPostprocessing(Type parent, Type child)
		{
			foreach (IChildAssociationPostprocessing item in aChildPostprocessings)
			{
				if (item.Parent.IsAssignableFrom(parent) && item.Child.IsAssignableFrom(child))
					return item.PostprocessDelegate;
			}

			return null;
		}
	}
}
