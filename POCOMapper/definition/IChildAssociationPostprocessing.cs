using System;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.common.parser;

namespace POCOMapper.definition
{
	internal interface IChildAssociationPostprocessing
	{
		Type Parent { get; }
		Type Child { get; }
		Delegate PostprocessDelegate { get; }
	}
}