using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.special
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

		public IEnumerable<Tuple<string, IMapping>> Children
		{
			get { return Enumerable.Empty<Tuple<string, IMapping>>(); }
		}

		public bool CanSynchronize
		{
			get { return aMappingAction != null; }
		}

		public bool CanMap
		{
			get { return aMappingFunc != null; }
		}

		public bool IsDirect
		{
			get { return false; }
		}

		#endregion

		#region Implementation of IMapping<TFrom,TTo>

		public TTo Map(TFrom from)
		{
			return this.aMappingFunc(from);
		}

		public void Synchronize(TFrom from, TTo to)
		{
			this.aMappingAction(from, to);
		}

		#endregion
	}
}
