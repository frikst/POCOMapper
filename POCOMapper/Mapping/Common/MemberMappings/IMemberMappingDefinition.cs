using System;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Common.Parser;

namespace KST.POCOMapper.Mapping.Common.MemberMappings
{
	internal interface IMemberMappingDefinition
	{
		PairedMembers CreateMapping(MappingImplementation allMappings, Type fromClass, Type toClass);
	}
}