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
		{
			get { return this.aMappingAction != null; }
		}

		public bool CanMap
		{
			get { return this.aMappingFunc != null; }
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
