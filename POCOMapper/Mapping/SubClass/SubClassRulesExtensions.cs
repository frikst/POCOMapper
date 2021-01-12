using KST.POCOMapper.Definition;

namespace KST.POCOMapper.Mapping.SubClass
{
	public static class SubClassRulesExtensions
	{
		public static SubClassMappingRules<TFrom, TTo> SubClassMappingRules<TFrom, TTo>(this IRulesDefinition<TFrom, TTo> definition)
		{
			return definition.Rules<SubClassMappingRules<TFrom, TTo>>();
		}

		public static SubClassMappingRules SubClassMappingRules(this IRulesDefinition definition)
		{
			return definition.Rules<SubClassMappingRules>();
		}
	}
}
