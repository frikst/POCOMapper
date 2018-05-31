using System;
using System.Collections.Generic;
using KST.POCOMapper.mapping.@base;

namespace KST.POCOMapper.mapping.common
{
	public interface ISubClassConversionMapping
	{
		Type From { get; }
		Type To { get; }

		IMapping Mapping { get; }
	}

	public interface ISubClassMapping : IMapping
	{
		IEnumerable<ISubClassConversionMapping> Conversions { get; }
	}
}
