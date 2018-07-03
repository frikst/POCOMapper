using System;

namespace KST.POCOMapper.Executor.TypeMappingDefinition
{
	interface IExactTypeMappingDefinition : ITypeMappingDefinition
	{
		Type From { get; }

		Type To { get; }
	}
}