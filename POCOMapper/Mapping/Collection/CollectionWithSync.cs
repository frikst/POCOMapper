using System;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Collection.Compiler;
using KST.POCOMapper.Mapping.MappingCompilaton;
using KST.POCOMapper.SpecialRules;

namespace KST.POCOMapper.Mapping.Collection
{
	public class CollectionWithSync<TFrom, TTo> : CollectionWithMap<TFrom, TTo>, IMappingWithSyncSupport<TFrom, TTo>
	{
		private readonly CollectionSynchronizationCompiler<TFrom, TTo> aSynchronizationExpression;

		internal CollectionWithSync(MappingDefinitionInformation mappingDefinition, IEqualityRules equalityRules)
			: base(mappingDefinition, equalityRules)
		{
			var itemMapping = mappingDefinition.UnresolvedMappings.GetUnresolvedMapping(EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType);

			var childPostprocessing = mappingDefinition.GetChildPostprocessing(typeof(TTo), EnumerableReflection<TTo>.ItemType);

			if (typeof(TTo).IsArray)
				this.aSynchronizationExpression = new ArraySynchronizationCompiler<TFrom, TTo>(itemMapping, equalityRules, childPostprocessing);
			else
				throw new NotImplementedException("Only array synchronization supported yet");
		}

		public bool SynchronizeCanChangeObject
			=> true;

		public TTo Synchronize(TFrom from, TTo to)
		{
			return this.aSynchronizationExpression.Synchronize(from, to);
		}
	}
}
