using System.Collections;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.SpecialRules;

namespace KST.POCOMapper.Mapping.Collection
{
	public class CollectionMappingRules<TFrom, TTo> : IMappingRules<TFrom, TTo>
		where TFrom : IEnumerable
		where TTo : IEnumerable
	{
		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingDefinitionInformation mappingDefinition)
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
