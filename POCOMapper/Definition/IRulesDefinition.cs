using KST.POCOMapper.mapping.@base;

namespace KST.POCOMapper.definition
{
	public interface IRulesDefinition
	{
		TRules Rules<TRules>() where TRules : class, IMappingRules, new();
	}

	public interface IRulesDefinition<TFrom, TTo>
	{
		TRules Rules<TRules>() where TRules : class, IMappingRules<TFrom, TTo>, new();
	}
}
