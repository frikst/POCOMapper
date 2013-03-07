using System;
using System.Collections.Generic;

namespace POCOMapper.mapping.@base
{
	public interface IMapping
	{
		IEnumerable<Tuple<string, IMapping>> Children { get; }

		bool CanSynchronize { get; }
		bool CanMap { get; }

		bool IsDirect { get; }

		bool SynchronizeCanChangeObject { get; }

		string MappingSource { get; }
		string SynchronizationSource { get; }
	}

	public interface IMapping<TFrom, TTo> : IMapping
	{
		TTo Map(TFrom from);
		TTo Synchronize(TFrom from, TTo to);
	}
}
