using System;

namespace KST.POCOMapper.definition
{
	internal interface IChildAssociationPostprocessing
	{
		Type Parent { get; }
		Type Child { get; }
		Delegate PostprocessDelegate { get; }
	}
}