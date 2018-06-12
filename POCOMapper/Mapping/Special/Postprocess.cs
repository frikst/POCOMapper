using System;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Special
{
	public class Postprocess<TFrom, TTo> : IMapping<TFrom, TTo>
	{
		private readonly IMapping<TFrom, TTo> aInnerMapping;
		private readonly Delegate aPostprocessDelegate;

		public Postprocess(IMapping<TFrom, TTo> innerMapping, Delegate postprocessDelegate)
		{
			this.aInnerMapping = innerMapping;
			this.aPostprocessDelegate = postprocessDelegate;
		}

		#region Implementation of IMapping

		public void Accept(IMappingVisitor visitor)
		{
			this.aInnerMapping.Accept(visitor);
		}

		public bool CanSynchronize
			=> this.aInnerMapping.CanSynchronize;

		public bool CanMap
			=> this.aInnerMapping.CanMap;

		public bool IsDirect
			=> false;

		public bool SynchronizeCanChangeObject
			=> false;

		public string MappingSource
			=> null;

		public string SynchronizationSource
			=> null;

		public Type From
			=> typeof(TFrom);

		public Type To
			=> typeof(TTo);

		#endregion

		#region Implementation of IMapping<TFrom,TTo>

		public TTo Map(TFrom from)
		{
			TTo ret = this.aInnerMapping.Map(from);
			this.aPostprocessDelegate.DynamicInvoke(from, ret);
			return ret;
		}

		public TTo Synchronize(TFrom from, TTo to)
		{
			to = this.aInnerMapping.Synchronize(from, to);
			this.aPostprocessDelegate.DynamicInvoke(from, to);
			return to;
		}

		#endregion
	}
}
