using System;

namespace KST.POCOMapper.Definition
{
	interface IExactMappingDefinition : IMappingDefinition
	{
		Type From { get; }

		Type To { get; }
	}
}