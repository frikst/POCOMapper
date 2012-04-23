using System;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.common.parser;

namespace POCOMapper.definition
{
	internal enum MappingType
	{
		ClassMapping,
		ContainerMapping
	}

	internal interface IMappingDefinition
	{
		IMapping CreateMapping(MappingImplementation allMappings, Type from, Type to);
		Type From { get; }
		Type To { get; }

		MappingType Type { get; }
	}

	internal interface IMemberMappingDefinition
	{
		PairedMembers CreateMapping(MappingImplementation allMappings);
	}

	internal interface IChildAssociationPostprocessing
	{
		Type Parent { get; }
		Type Child { get; }
		Delegate PostprocessDelegate { get; }
	}
}