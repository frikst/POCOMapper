using System;
using System.Collections;
using POCOMapper.definition;

namespace POCOMapper.mapping.collection
{
	public static class CollectionMappingRulesExtensions
	{
		public static CollectionMappingRules CollectionMappingRules(this IRulesDefinition definition)
		{
			return definition.Rules<CollectionMappingRules>();
		}

		public static CollectionMappingRules<TFrom, TTo> CollectionMappingRules<TFrom, TTo>(this IRulesDefinition<TFrom, TTo> definition)
			where TFrom : IEnumerable
			where TTo : IEnumerable
		{
			return definition.Rules<CollectionMappingRules<TFrom, TTo>>();
		}
	}
}
