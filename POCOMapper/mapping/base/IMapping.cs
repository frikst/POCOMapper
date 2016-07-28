using System;
using POCOMapper.visitor;

namespace POCOMapper.mapping.@base
{
	public interface IMapping
	{
		void Accept(IMappingVisitor visitor);

		bool CanSynchronize { get; }
		bool CanMap { get; }

		bool IsDirect { get; }

		bool SynchronizeCanChangeObject { get; }

		string MappingSource { get; }
		string SynchronizationSource { get; }

		Type From { get; }
		Type To { get; }
	}

	public interface IMapping<TFrom, TTo> : IMapping
	{
		TTo Map(TFrom from);
		TTo Synchronize(TFrom from, TTo to);
	}
}
