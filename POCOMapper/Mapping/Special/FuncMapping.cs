using System;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Special
{
	public class FuncMapping<TFrom, TTo> : IMapping<TFrom, TTo>
	{
		private readonly Func<TFrom, TTo> aMappingFunc;
		private readonly Action<TFrom, TTo> aMappingAction;

		public FuncMapping(Func<TFrom, TTo> mappingFunc)
		{
			this.aMappingFunc = mappingFunc;
			this.aMappingAction = null;
		}

		public FuncMapping(Action<TFrom, TTo> mappingAction)
		{
			this.aMappingFunc = null;
			this.aMappingAction = mappingAction;
		}

		#region Implementation of IMapping

		public void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public bool CanSynchronize
			=> this.aMappingAction != null;

		public bool CanMap
			=> this.aMappingFunc != null;

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
			return this.aMappingFunc(from);
		}

		public TTo Synchronize(TFrom from, TTo to)
		{
			this.aMappingAction(from, to);
			return to;
		}

		#endregion
	}
}
