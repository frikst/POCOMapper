using KST.POCOMapper.Executor;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.SpecialRules;

namespace KST.POCOMapper.Mapping.Collection
{
	public class CollectionMappingRules : IMappingRules
	{
		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingDefinitionInformation mappingDefinition)
		{
			var equalityRules = mappingDefinition.SpecialRules.GetRules<IEqualityRules>(EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType);

			if (typeof(TTo).IsArray && equalityRules != null)
				return new CollectionWithSync<TFrom, TTo>(mappingDefinition, equalityRules);
			else
				return new CollectionWithMap<TFrom, TTo>(mappingDefinition);
		}

		#endregion
	}
}
