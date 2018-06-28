using System;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Base
{
	public interface IMapping
	{
		void Accept(IMappingVisitor visitor);
		bool CanMap { get; }

		bool IsDirect { get; }

		Type From { get; }
		Type To { get; }
	}

	public interface IMapping<TFrom, TTo> : IMapping
	{
		TTo Map(TFrom from);
	}
}
