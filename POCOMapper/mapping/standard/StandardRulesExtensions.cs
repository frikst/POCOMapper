using System;
using POCOMapper.definition;

namespace POCOMapper.mapping.standard
{
	public static class StandardRulesExtensions
	{
		public static CastRules CastRules(this IRulesDefinition definition)
		{
			return definition.Rules<CastRules>();
		}

		public static CastRules<TFrom, TTo> CastRules<TFrom, TTo>(this IRulesDefinition<TFrom, TTo> definition)
			where TFrom : struct
			where TTo : struct
		{
			return definition.Rules<CastRules<TFrom, TTo>>();
		}

		public static CopyRules CopyRules(this IRulesDefinition definition)
		{
			return definition.Rules<CopyRules>();
		}

		public static CopyRules<TFromTTo> CopyRules<TFromTTo>(this IRulesDefinition<TFromTTo, TFromTTo> definition)
		{
			return definition.Rules<CopyRules<TFromTTo>>();
		}

		public static ParseRules ParseRules(this IRulesDefinition definition)
		{
			return definition.Rules<ParseRules>();
		}

		public static ParseRules<TTo> ParseRules<TTo>(this IRulesDefinition<string, TTo> definition)
		{
			return definition.Rules<ParseRules<TTo>>();
		}

		public static ToStringRules ToStringRules(this IRulesDefinition definition)
		{
			return definition.Rules<ToStringRules>();
		}

		public static ToStringRules<TFrom> ToStringRules<TFrom>(this IRulesDefinition<TFrom, string> definition)
		{
			return definition.Rules<ToStringRules<TFrom>>();
		}
	}
}
