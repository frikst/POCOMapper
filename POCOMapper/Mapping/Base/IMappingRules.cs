using KST.POCOMapper.Definition;

namespace KST.POCOMapper.Mapping.Base
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
