using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMapper.conventions;
using POCOMapper.exceptions;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.standard;
using POCOMapper.visitor;

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
		private readonly List<IChildAssociationPostprocessing> aChildPostprocessings;

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
		public void Synchronize<TFrom, TTo>(TFrom from, ref TTo to)
		{
			IMapping<TFrom, TTo> mapping = this.GetMapping<TFrom, TTo>();

			if (mapping == null)
				throw new UnknownMapping(typeof(TFrom), typeof(TTo));

			if (!mapping.CanSynchronize)
				throw new CantMap(string.Format("Can't synchronize {0} to {1}, mapping object does not support synchronization", typeof(TFrom).Name, typeof(TTo).Name));

			if (mapping.SynchronizeCanChangeObject)
				to = mapping.Synchronize(from, to);
			else
				mapping.Synchronize(from, to);
		}

		public void AcceptForAll(IMappingVisitor visitor)
		{
			visitor.Begin();

			bool first = true;
			foreach (IMappingDefinition mappingDefinition in this.aMappingDefinitions)
			{
				Tuple<Type, Type> key = mappingDefinition.GetKey();

				if (key != null)
				{
					IMapping mapping = this.GetMapping(key.Item1, key.Item2);

					if (!first)
						visitor.Next();
					first = false;

					mapping.Accept(visitor);
				}
			}

			visitor.End();
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
