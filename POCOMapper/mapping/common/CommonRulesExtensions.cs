using System;
using POCOMapper.definition;

namespace POCOMapper.mapping.common
{
	public static class CommonRulesExtensions
	{
		public static ObjectMappingRules<TFrom, TTo> ObjectMappingRules<TFrom, TTo>(this IRulesDefinition<TFrom, TTo> definition)
		{
			return definition.Rules<ObjectMappingRules<TFrom, TTo>>();
		}

		public static SubClassMappingRules<TFrom, TTo> SubClassMappingRules<TFrom, TTo>(this IRulesDefinition<TFrom, TTo> definition)
		{
			return definition.Rules<SubClassMappingRules<TFrom, TTo>>();
		}
	}
}
