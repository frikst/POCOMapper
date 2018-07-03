using System;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.SpecialRules;

namespace KST.POCOMapper.Executor.TypeMappingDefinition
{
	internal interface ITypeMappingDefinition
	{
		IMapping CreateMapping(MappingDefinitionInformation mappingDefinition, Type from, Type to);

		TRules GetSpecialRules<TRules>()
			where TRules : class, ISpecialRules;

		bool IsDefinedFor(MappingDefinitionInformation mappingDefinition, Type @from, Type to);

		int Priority { get; }

		bool Visitable { get; }
	}
}
