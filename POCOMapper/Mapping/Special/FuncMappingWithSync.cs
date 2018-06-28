using System;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Special
{
	public class FuncMappingWithSync<TFrom, TTo> : FuncMappingWithMap<TFrom, TTo>, IMappingWithSyncSupport<TFrom, TTo>
	{
		private readonly Action<TFrom, TTo> aMappingAction;

		public FuncMappingWithSync(Func<TFrom, TTo> mappingFunc, Action<TFrom, TTo> mappingAction)
			: base(mappingFunc)
		{
			this.aMappingAction = mappingAction;
		}

		public bool SynchronizeCanChangeObject
			=> false;

		public TTo Synchronize(TFrom from, TTo to)
		{
			this.aMappingAction(from, to);
			return to;
		}
	}
}
