using System;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Special
{
	public class PostprocessWithMap<TFrom, TTo> : IMapping<TFrom, TTo>
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
			this.aInnerMapping.Accept(visitor);
		}

		public bool CanMap
			=> this.aInnerMapping.CanMap;

		public bool IsDirect
			=> false;

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
	}
}
