using System;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Definition
{
	internal interface IMappingDefinition
	{
		IMapping CreateMapping(MappingImplementation allMappings, Type from, Type to);

		bool IsFrom(Type from);
		bool IsTo(Type to);

		int Priority { get; }
	}
}
