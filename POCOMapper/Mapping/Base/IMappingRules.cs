using KST.POCOMapper.Executor;

namespace KST.POCOMapper.Mapping.Base
{
	public interface IMappingRules
	{
		IMapping<TFrom, TTo> Create<TFrom, TTo>(MappingDefinitionInformation mappingDefinition);
	}

	public interface IMappingRules<TFrom, TTo>
	{
		IMapping<TFrom, TTo> Create(MappingDefinitionInformation mappingDefinition);
	}
}
