using System;

namespace KST.POCOMapper.Definition.ChildProcessingDefinition
{
	internal interface IChildAssociationPostprocessing
	{
		Type Parent { get; }
		Type Child { get; }
		Delegate PostprocessDelegate { get; }
	}
}