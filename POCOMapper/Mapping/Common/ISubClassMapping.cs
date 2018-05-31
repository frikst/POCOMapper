using System;
using System.Collections.Generic;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.common
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
