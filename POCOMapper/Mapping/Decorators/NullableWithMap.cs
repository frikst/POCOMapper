using System;
using System.Collections.Generic;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Decorators
{
	public class NullableWithMap<TFrom, TTo> : IMapping<TFrom, TTo>, IMappingWithSpecialComparision<TFrom, TTo>, IDecoratorMapping
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

        public bool MapEqual(TFrom from, TTo to)
        {
            if (from == null)
                return to == null;
            if (to == null)
                return false;

            return this.aInnerMapping.DoMapEqualCheck(from, to);
        }

		public IMapping DecoratedMapping
			=> this.aInnerMapping;
	}
}