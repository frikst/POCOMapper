using System;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.common.parser;

namespace POCOMapper.definition
{
	internal interface IMappingDefinition
	{
		IMapping CreateMapping(MappingImplementation allMappings, Type from, Type to);

		bool IsFrom(Type from);
		bool IsTo(Type to);

		Tuple<Type, Type> GetKey();

		int Priority { get; }
	}
}