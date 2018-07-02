using KST.POCOMapper.Definition;

namespace KST.POCOMapper.Mapping.Special
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

		public static PostprocessRules PostprocessRules(this IRulesDefinition definition)
		{
			return definition.Rules<PostprocessRules>();
		}

		public static NullableRules<TFrom, TTo> NullableRules<TFrom, TTo>(this IRulesDefinition<TFrom, TTo> definition)
			where TFrom : class
			where TTo : class
		{
			return definition.Rules<NullableRules<TFrom, TTo>>();
		}

		public static NullableRules NullableRules(this IRulesDefinition definition)
		{
			return definition.Rules<NullableRules>();
		}
	}
}
