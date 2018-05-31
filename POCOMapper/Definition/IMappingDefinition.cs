using System;
using KST.POCOMapper.mapping.@base;

namespace KST.POCOMapper.definition
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