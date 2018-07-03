using System;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Decorators
{
	public class PostprocessWithSync<TFrom, TTo> : IMappingWithSyncSupport<TFrom, TTo>
	{
		private readonly IMappingWithSyncSupport<TFrom, TTo> aInnerMapping;
		private readonly Action<TFrom, TTo> aPostprocessDelegate;

		public PostprocessWithSync(IMappingWithSyncSupport<TFrom, TTo> innerMapping, Action<TFrom, TTo> postprocessDelegate)
		{
			this.aInnerMapping = innerMapping;
			this.aPostprocessDelegate = postprocessDelegate;
		}

		#region Implementation of IMapping

		public void Accept(IMappingVisitor visitor)
		{
			this.aInnerMapping.Accept(visitor);
		}

		public bool IsDirect
			=> false;

		public bool SynchronizeCanChangeObject
			=> false;

		public Type From
			=> typeof(TFrom);

		public Type To
			=> typeof(TTo);

		#endregion

		#region Implementation of IMapping<TFrom,TTo>

		public TTo Map(TFrom from)
		{
			TTo ret = this.aInnerMapping.Map(from);
			this.aPostprocessDelegate(from, ret);
			return ret;
		}

		public TTo Synchronize(TFrom from, TTo to)
		{
			var mappingWithSync = this.aInnerMapping as IMappingWithSyncSupport<TFrom, TTo>;

			if (mappingWithSync == null)
				throw new InvalidOperationException("Synchronization not implemented");

			to = mappingWithSync.Synchronize(from, to);
			this.aPostprocessDelegate(from, to);
			return to;
		}

		#endregion
	}
}
