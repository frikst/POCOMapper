using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMapper.mapping.@base;
using POCOMapper.visitor;

namespace POCOMapper.mapping.special
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
		{
			get { return this.aInnerMapping.CanSynchronize; }
		}

		public bool CanMap
		{
			get { return this.aInnerMapping.CanMap; }
		}

		public bool IsDirect
		{
			get { return false; }
		}

		public bool SynchronizeCanChangeObject
		{
			get { return false; }
		}

		public string MappingSource
		{
			get { return null; }
		}

		public string SynchronizationSource
		{
			get { return null; }
		}

		public Type From
		{
			get { return typeof(TFrom); }
		}

		public Type To
		{
			get { return typeof(TTo); }
		}

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
