using System;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Definition.TypeMappingDefinition
{
	internal interface ITypeMappingDefinition
	{
		IMapping CreateMapping(MappingDefinitionInformation mappingDefinition, Type from, Type to);

		bool IsDefinedFor(MappingDefinitionInformation mappingDefinition, Type @from, Type to);

		int Priority { get; }

		bool Visitable { get; }
	}
}
