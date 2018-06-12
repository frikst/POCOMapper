using System;
using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Conventions;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Standard;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Definition
{
	/// <summary>
	/// One defined and compiled mappings set.
	/// </summary>
	public class MappingImplementation
	{
		private readonly List<IMappingDefinition> aMappingDefinitions;
		private readonly Dictionary<(Type From, Type To), IMapping> aMappings;
		private readonly List<IChildAssociationPostprocessing> aChildPostprocessings;

		internal MappingImplementation(IEnumerable<IMappingDefinition> mappingDefinitions, IEnumerable<IChildAssociationPostprocessing> childPostprocessings, NamingConventions fromConventions, NamingConventions toConventions)
		{
			this.aMappings = new Dictionary<(Type, Type), IMapping>();

			this.aMappingDefinitions = mappingDefinitions.OrderBy(x => x.Priority).ToList();

			this.aChildPostprocessings = childPostprocessings.ToList();

			this.FromConventions = fromConventions;
			this.ToConventions = toConventions;
		}

		internal NamingConventions FromConventions { get; }
		internal NamingConventions ToConventions { get; }

		/// <summary>
		/// Finds the mapping from the type specified by the <paramref name="from"/> parameter to the type specified
		/// by the <paramref name="to"/> parameter.
		/// </summary>
		/// <param name="from">Class from the source model.</param>
		/// <param name="to">Class from the destination model.</param>
		/// <returns>The mapping specified by the parameters.</returns>
		public IMapping GetMapping(Type from, Type to)
		{
			if (this.aMappings.ContainsKey((from, to)))
				return this.aMappings[(from, to)];

			foreach (IMappingDefinition mappingDefinition in this.aMappingDefinitions)
			{
				if (mappingDefinition.IsFrom(from) && mappingDefinition.IsTo(to))
				{
					IMapping mapping = mappingDefinition.CreateMapping(this, from, to);
					this.aMappings[(from, to)] = mapping;
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
			=> (IMapping<TFrom, TTo>)this.GetMapping(typeof(TFrom), typeof(TTo));

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
				throw new UnknownMappingException(typeof(TFrom), typeof(TTo));

			if (!mapping.CanMap)
				throw new CantMapException($"Can't map {typeof(TFrom).Name} to {typeof(TTo).Name}, mapping object does not support simple mapping");

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
				throw new UnknownMappingException(typeof(TFrom), typeof(TTo));

			if (!mapping.CanSynchronize)
				throw new CantMapException($"Can't synchronize {typeof(TFrom).Name} to {typeof(TTo).Name}, mapping object does not support synchronization");

			if (mapping.SynchronizeCanChangeObject)
				to = mapping.Synchronize(from, to);
			else
				mapping.Synchronize(from, to);
		}

		public void AcceptForAll(IMappingVisitor visitor)
		{
			visitor.Begin();

			bool first = true;
			foreach (var mappingDefinition in this.aMappingDefinitions.OfType<IExactMappingDefinition>())
			{
				var mapping = this.GetMapping(mappingDefinition.From, mappingDefinition.To);

				if (!first)
					visitor.Next();
				first = false;

				mapping.Accept(visitor);
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
			=> this.GetMapping(from, to).MappingSource;

		/// <summary>
		/// Returns source for compiled synchronization.
		/// </summary>
		/// <typeparam name="TFrom">Class from the source model.</typeparam>
		/// <typeparam name="TTo">Class from the destination model.</typeparam>
		/// <returns>Source code, or null if no source code is generated by mapping</returns>
		public string GetSynchronizationSource<TFrom, TTo>()
			=> this.GetSynchronizationSource(typeof(TFrom), typeof(TTo));

		/// <summary>
		/// Returns source for compiled synchronization.
		/// </summary>
		/// <param name="from">Class from the source model.</param>
		/// <param name="to">Class from the destination model.</param>
		/// <returns>Source code, or null if no source code is generated by mapping</returns>
		public string GetSynchronizationSource(Type from, Type to)
			=> this.GetMapping(from, to).SynchronizationSource;

		internal Delegate GetChildPostprocessing(Type parent, Type child)
		{
			foreach (IChildAssociationPostprocessing item in this.aChildPostprocessings)
			{
				if (item.Parent.IsAssignableFrom(parent) && item.Child.IsAssignableFrom(child))
					return item.PostprocessDelegate;
			}

			return null;
		}
	}
}
