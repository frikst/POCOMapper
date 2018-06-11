using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Definition
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
