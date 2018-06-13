using KST.POCOMapper.Definition;

namespace KST.POCOMapper.Mapping.Object
{
	public static class ObjectRulesExtensions
	{
		public static ObjectMappingRules<TFrom, TTo> ObjectMappingRules<TFrom, TTo>(this IRulesDefinition<TFrom, TTo> definition)
		{
			return definition.Rules<ObjectMappingRules<TFrom, TTo>>();
		}

		public static ObjectMappingRules ObjectMappingRules(this IRulesDefinition definition)
		{
			return definition.Rules<ObjectMappingRules>();
		}
	}
}
