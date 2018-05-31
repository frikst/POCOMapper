using KST.POCOMapper.definition;

namespace KST.POCOMapper.mapping.common
{
	public static class CommonRulesExtensions
	{
		public static ObjectMappingRules<TFrom, TTo> ObjectMappingRules<TFrom, TTo>(this IRulesDefinition<TFrom, TTo> definition)
		{
			return definition.Rules<ObjectMappingRules<TFrom, TTo>>();
		}

		public static ObjectMappingRules ObjectMappingRules(this IRulesDefinition definition)
		{
			return definition.Rules<ObjectMappingRules>();
		}

		public static SubClassMappingRules<TFrom, TTo> SubClassMappingRules<TFrom, TTo>(this IRulesDefinition<TFrom, TTo> definition)
		{
			return definition.Rules<SubClassMappingRules<TFrom, TTo>>();
		}
	}
}
