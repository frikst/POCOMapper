using System;

namespace KST.POCOMapper.Definition.TypeMappingDefinition
{
	interface IExactTypeMappingDefinition : ITypeMappingDefinition
	{
		Type From { get; }

		Type To { get; }
	}
}