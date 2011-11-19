using System;
using POCOMapper.mapping.@base;

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
}