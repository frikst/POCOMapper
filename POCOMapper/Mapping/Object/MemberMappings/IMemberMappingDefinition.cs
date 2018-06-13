using System;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Object.Parser;

namespace KST.POCOMapper.Mapping.Object.MemberMappings
{
	internal interface IMemberMappingDefinition
	{
		PairedMembers CreateMapping(MappingImplementation allMappings, Type fromClass, Type toClass);
	}
}