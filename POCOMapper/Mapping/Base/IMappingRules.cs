using KST.POCOMapper.definition;

namespace KST.POCOMapper.mapping.@base
{
	public interface IMappingRules
	{
		IMapping<TFrom, TTo> Create<TFrom, TTo>(MappingImplementation mapping);
	}

	public interface IMappingRules<TFrom, TTo>
	{
		IMapping<TFrom, TTo> Create(MappingImplementation mapping);
	}
}
