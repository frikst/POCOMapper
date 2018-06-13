using System;
using System.Collections.Generic;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.SubClass
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
