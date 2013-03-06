using System;
using POCOMapper.definition;

namespace POCOMapper.mapping.special
{
	public static class SpecialRulesExtensions
	{
		public static FuncMappingRules<TFrom, TTo> FuncMappingRules<TFrom, TTo>(this IRulesDefinition<TFrom, TTo> definition)
		{
			return definition.Rules<FuncMappingRules<TFrom, TTo>>();
		}

		public static PostprocessRules<TFrom, TTo> PostprocessRules<TFrom, TTo>(this IRulesDefinition<TFrom, TTo> definition)
		{
			return definition.Rules<PostprocessRules<TFrom, TTo>>();
		}

	}
}
