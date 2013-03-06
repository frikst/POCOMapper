using POCOMapper.definition;
using POCOMapper.mapping.common.parser;

namespace POCOMapper.mapping.common.memberMappings
{
	internal interface IMemberMappingDefinition
	{
		PairedMembers CreateMapping(MappingImplementation allMappings);
	}
}