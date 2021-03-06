﻿using KST.POCOMapper.Definition;

namespace KST.POCOMapper.Mapping.Standard
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

		public static CustomMappingRules<TFrom, TTo> CustomMappingRules<TFrom, TTo>(this IRulesDefinition<TFrom, TTo> definition)
		{
			return definition.Rules<CustomMappingRules<TFrom, TTo>>();
		}

		public static CustomMappingRules CustomMappingRules(this IRulesDefinition definition)
		{
			return definition.Rules<CustomMappingRules>();
		}
		public static FuncMappingRules<TFrom, TTo> FuncMappingRules<TFrom, TTo>(this IRulesDefinition<TFrom, TTo> definition)
		{
			return definition.Rules<FuncMappingRules<TFrom, TTo>>();
		}

	}
}
