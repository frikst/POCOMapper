using System;
using System.Collections.Generic;

namespace POCOMapper.mapping.@base
{
	public interface IMapping
	{
		IEnumerable<Tuple<string, IMapping>> Children { get; }
	}

	public interface IMapping<TFrom, TTo> : IMapping
	{
		TTo Map(TFrom from);
		void Synchronize(TFrom from, TTo to);
	}
}
