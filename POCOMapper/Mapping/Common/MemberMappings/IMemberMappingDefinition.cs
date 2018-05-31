using System;
using KST.POCOMapper.definition;
using KST.POCOMapper.mapping.common.parser;

namespace KST.POCOMapper.mapping.common.memberMappings
{
	internal interface IMemberMappingDefinition
	{
		PairedMembers CreateMapping(MappingImplementation allMappings, Type fromClass, Type toClass);
	}
}