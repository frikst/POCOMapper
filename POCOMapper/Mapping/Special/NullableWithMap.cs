using System;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Special
{
	public class NullableWithMap<TFrom, TTo> : IMapping<TFrom, TTo>, IDecoratorMapping
		where TFrom : class
		where TTo : class
	{
		private readonly IMapping<TFrom, TTo> aInnerMapping;

		public NullableWithMap(IMapping<TFrom, TTo> innerMapping)
		{
			this.aInnerMapping = innerMapping;
		}

		public void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public bool IsDirect
			=> false;

		public Type From
			=> typeof(TFrom);

		public Type To
			=> typeof(TTo);

		public TTo Map(TFrom from)
		{
			if (from == null)
				return null;

			return this.aInnerMapping.Map(from);
		}

		public IMapping DecoratedMapping
			=> this.aInnerMapping;
	}
}