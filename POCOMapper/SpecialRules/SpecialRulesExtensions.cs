using KST.POCOMapper.Definition;

namespace KST.POCOMapper.SpecialRules
{
    public static class SpecialRulesExtensions
    {
	    public static EqualityRules EqualityRules(this IRulesDefinition definition)
	    {
		    return definition.Rules<EqualityRules>();
	    }

	    public static EqualityRules<TFrom, TTo> EqualityRules<TFrom, TTo>(this IRulesDefinition<TFrom, TTo> definition)
	    {
		    return definition.Rules<EqualityRules<TFrom, TTo>>();
	    }
    }
}
