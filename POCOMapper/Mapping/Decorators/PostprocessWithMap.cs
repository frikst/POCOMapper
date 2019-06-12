using System;
using System.Collections.Generic;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Decorators
{
	public class PostprocessWithMap<TFrom, TTo> : IMapping<TFrom, TTo>, IMappingWithSpecialComparision<TFrom, TTo>, IDecoratorMapping
	{
		private readonly IMapping<TFrom, TTo> aInnerMapping;
		private readonly Action<TFrom, TTo> aPostprocessDelegate;

		public PostprocessWithMap(IMapping<TFrom, TTo> innerMapping, Action<TFrom, TTo> postprocessDelegate)
		{
			this.aInnerMapping = innerMapping;
			this.aPostprocessDelegate = postprocessDelegate;
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
			TTo ret = this.aInnerMapping.Map(from);
			this.aPostprocessDelegate(from, ret);
			return ret;
		}

        public bool MapEqual(TFrom from, TTo to)
        {
            return this.aInnerMapping.DoMapEqualCheck(from, to);
        }

		#region Implementation of IDecoratorMapping

		public IMapping DecoratedMapping
			=> this.aInnerMapping;

		#endregion
	}
}
