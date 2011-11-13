using System;
using System.Collections.Generic;

namespace POCOMapper.mapping.@base
{
	public interface IMapping
	{
		IEnumerable<Tuple<string, IMapping>> Children { get; }
	}

	public interface IMapping<in TFrom, out TTo> : IMapping
	{
		TTo Map(TFrom from);
	}
}
