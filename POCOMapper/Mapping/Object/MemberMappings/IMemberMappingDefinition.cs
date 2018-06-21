using System;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Object.Parser;

namespace KST.POCOMapper.Mapping.Object.MemberMappings
{
	internal interface IMemberMappingDefinition
	{
		PairedMembers CreateMapping(MappingDefinitionInformation mappingDefinition, Type fromClass, Type toClass);
	}
}