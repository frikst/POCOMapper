using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMapper.conventions;
using POCOMapper.exceptions;
using POCOMapper.mapping.@base;

namespace POCOMapper.definition
{
	/// <summary>
	/// One defined and compiled mappings set.
	/// </summary>
	public class MappingImplementation
	{
		private readonly Dictionary<Tuple<Type, Type>, IMappingDefinition> aClassMappingDefinitions;
		private readonly Dictionary<Tuple<Type, Type>, IMappingDefinition> aContainerMappingDefinitions;
		private readonly Dictionary<Tuple<Type, Type>, IMapping> aMappings;
		private List<IChildAssociationPostprocessing> aChildPostprocessings;

		internal MappingImplementation(IEnumerable<IMappingDefinition> mappingDefinitions, IEnumerable<IChildAssociationPostprocessing> childPostprocessings, Conventions fromConventions, Conventions toConventions)
		{
			this.aMappings = new Dictionary<Tuple<Type, Type>, IMapping>();
			this.aClassMappingDefinitions = new Dictionary<Tuple<Type, Type>, IMappingDefinition>();
			this.aContainerMappingDefinitions = new Dictionary<Tuple<Type, Type>, IMappingDefinition>();

			foreach (IMappingDefinition def in mappingDefinitions)
			{
				if (def.Type == MappingType.ClassMapping)
					this.aClassMappingDefinitions[new Tuple<Type, Type>(def.From, def.To)] = def;
				else
					this.aContainerMappingDefinitions[new Tuple<Type, Type>(def.From, def.To)] = def;
			}

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
			
			if (this.aClassMappingDefinitions.ContainsKey(key))
			{
				IMapping mapping = this.aClassMappingDefinitions[key].CreateMapping(this, from, to);
				this.aMappings[key] = mapping;
				return mapping;
			}

			bool fromIsEnumerable = from.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				|| (from.IsGenericType && from.GetGenericTypeDefinition() == typeof(IEnumerable<>));
			bool toIsEnumerable = to.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				|| (to.IsGenericType && to.GetGenericTypeDefinition() == typeof(IEnumerable<>));

			if (fromIsEnumerable && toIsEnumerable)
			{
				IEnumerable<Type> fromPosibilities = this.GetGenericPosibilities(from);
				IEnumerable<Type> toPosibilities = this.GetGenericPosibilities(to);

				foreach (Type fromBase in fromPosibilities)
				{
					foreach (Type toBase in toPosibilities)
					{
						Tuple<Type, Type> baseKey = new Tuple<Type, Type>(fromBase, toBase);

						if (aContainerMappingDefinitions.ContainsKey(baseKey))
						{
							IMapping mapping = this.aContainerMappingDefinitions[baseKey].CreateMapping(this, from, to);
							this.aMappings[key] = mapping;
							return mapping;
						}
					}
				}
			}

			return null;
		}

		private IEnumerable<Type> GetGenericPosibilities(Type type)
		{
			Type tempType = type;
			bool orig = true;

			while (tempType != null)
			{
				if (tempType.IsArray)
				{
					tempType = typeof(T[]);
					break;
				}
				if (tempType.IsGenericType && tempType.GetGenericArguments().Length == 1)
				{
					tempType = tempType.GetGenericTypeDefinition().MakeGenericType(typeof(T));
					break;
				}
				tempType = tempType.BaseType;
				orig = false;
			}

			if (tempType == null)
				return new List<Type> { type };
			else if (!orig)
			{
				if ((typeof (IEnumerable<T>).IsAssignableFrom(tempType)))
					return new List<Type> {type, typeof (IEnumerable<T>)};
				else
					return new List<Type> {type};
			}
			else
			{
				List<Type> fromPosibilities = new List<Type> { type };
				for (Type fromBase = tempType; fromBase != null; fromBase = fromBase.BaseType)
					if ((typeof(IEnumerable<T>).IsAssignableFrom(fromBase)))
						fromPosibilities.Add(fromBase);
				fromPosibilities.AddRange(tempType.GetInterfaces().Where(x => typeof(IEnumerable<T>).IsAssignableFrom(x)));
				return fromPosibilities;
			}
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

			foreach (Tuple<Type, Type> key in this.aClassMappingDefinitions.Keys)
			{
				IMapping mapping = this.GetMapping(key.Item1, key.Item2);
				StringBuilder output = new StringBuilder();
				this.MappingToString(mapping, output, 1, toIgnore);
				results.Add(new Tuple<IMapping,StringBuilder>(mapping, output));
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
