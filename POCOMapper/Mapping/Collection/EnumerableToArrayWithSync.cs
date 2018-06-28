using System;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Collection.Compiler;

namespace KST.POCOMapper.Mapping.Collection
{
	public class EnumerableToArrayWithSync<TFrom, TTo> : EnumerableToArrayWithMap<TFrom, TTo>, IMappingWithSyncSupport<TFrom, TTo>
	{
		private readonly ArraySynchronizationCompiler<TFrom, TTo> aSynchronizationExpression;

		public EnumerableToArrayWithSync(MappingDefinitionInformation mappingDefinition, Delegate selectIdFrom, Delegate selectIdTo)
			: base(mappingDefinition)
		{
			var itemMapping = mappingDefinition.UnresolvedMappings.GetUnresolvedMapping(EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType);

			var childPostprocessing = mappingDefinition.GetChildPostprocessing(typeof(TTo), EnumerableReflection<TTo>.ItemType);

			this.aSynchronizationExpression = new ArraySynchronizationCompiler<TFrom, TTo>(itemMapping, selectIdFrom, selectIdTo, childPostprocessing);
		}

		public bool SynchronizeCanChangeObject
			=> true;

		public TTo Synchronize(TFrom from, TTo to)
		{
			return this.aSynchronizationExpression.Synchronize(from, to);
		}
	}
}
