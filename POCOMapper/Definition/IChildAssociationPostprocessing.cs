using System;

namespace KST.POCOMapper.Definition
{
	internal interface IChildAssociationPostprocessing
	{
		Type Parent { get; }
		Type Child { get; }
		Delegate PostprocessDelegate { get; }
	}
}