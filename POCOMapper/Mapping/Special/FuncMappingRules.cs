﻿using System;
using KST.POCOMapper.definition;
using KST.POCOMapper.exceptions;
using KST.POCOMapper.mapping.@base;

namespace KST.POCOMapper.mapping.special
{
	public class FuncMappingRules<TFrom, TTo> : IMappingRules<TFrom, TTo>
	{
		private Func<TFrom, TTo> aMappingFunc;
		private Action<TFrom, TTo> aMappingAction;

		public FuncMappingRules()
		{
			this.aMappingFunc = null;
			this.aMappingAction = null;
		}

		public void Using(Func<TFrom, TTo> mappingFunc)
		{
			this.aMappingFunc = mappingFunc;
		}

		public void Using(Action<TFrom, TTo> mappingAction)
		{
			this.aMappingAction = mappingAction;
		}

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingImplementation mapping)
		{
			if (this.aMappingFunc != null)
				return new FuncMapping<TFrom, TTo>(this.aMappingFunc);
			else if (this.aMappingAction != null)
				return new FuncMapping<TFrom, TTo>(this.aMappingAction);
			else
				throw new InvalidMapping("Function mapping without the mapping function defined");
		}

		#endregion
	}
}
