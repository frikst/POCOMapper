using System.Collections.Generic;
using KST.POCOMapper.Conventions;
using KST.POCOMapper.Definition.ChildProcessingDefinition;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Executor.TypeMappingDefinition;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Executor
{
	/// <summary>
	/// One defined and compiled mappings set.
	/// </summary>
	public class MappingExecutor
	{
		private readonly MappingDefinitionInformation aMappingDefinition;

		internal MappingExecutor(IEnumerable<ITypeMappingDefinition> mappingDefinitions, IEnumerable<IChildAssociationPostprocessing> childPostprocessings, NamingConventions fromConventions, NamingConventions toConventions)
		{
			this.aMappingDefinition = new MappingDefinitionInformation(mappingDefinitions, childPostprocessings, fromConventions, toConventions);
		}

		public MappingContainer Mappings
			=> this.aMappingDefinition.Mappings;

		/// <summary>
		/// Map the instance of the class from the source model onto the new instance of the class from the destination model.
		/// </summary>
		/// <typeparam name="TFrom">Class from the source model.</typeparam>
		/// <typeparam name="TTo">Class from the destination model.</typeparam>
		/// <param name="from">Instance that should be mapped.</param>
		/// <returns>New mapped instance.</returns>
		public TTo Map<TFrom, TTo>(TFrom from)
		{
			IMapping<TFrom, TTo> mapping = this.Mappings.GetMapping<TFrom, TTo>();
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
			var mapping = this.Mappings.GetMapping<TFrom, TTo>() as IMappingWithSyncSupport<TFrom, TTo>;

			if (mapping == null)
				throw new CantMapException($"Can't synchronize {typeof(TFrom).Name} to {typeof(TTo).Name}, mapping object does not support synchronization");

			if (mapping.SynchronizeCanChangeObject)
				to = mapping.Synchronize(from, to);
			else
				mapping.Synchronize(from, to);
		}

        /// <summary>
        /// Compare two objects for map-equality
        /// </summary>
        /// <typeparam name="TFrom">Class from the source model.</typeparam>
        /// <typeparam name="TTo">Class from the destination model.</typeparam>
        /// <param name="from">Instance to compare from source model.</param>
        /// <param name="to">Instance to compare from destination model.</param>
        public bool MapEqual<TFrom, TTo>(TFrom from, TTo to)
        {
            var mapping = this.Mappings.GetMapping<TFrom, TTo>();

            if (mapping is IMappingWithSpecialComparision<TFrom, TTo> specialComparision)
                return specialComparision.MapEqual(from, to);

            var mappedFrom = mapping.Map(from);
            return EqualityComparer<TTo>.Default.Equals(mappedFrom, to);
        }
	}
}
