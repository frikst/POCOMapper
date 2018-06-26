using System;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Special
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

		public void Using(Func<TFrom, TTo> mappingFunc, Action<TFrom, TTo> mappingAction)
		{
			this.aMappingFunc = mappingFunc;
			this.aMappingAction = mappingAction;
		}

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingDefinitionInformation mappingDefinition)
		{
			if (this.aMappingFunc == null)
				throw new InvalidMappingException("Function mapping without the mapping function defined");

			if (this.aMappingAction != null)
				return new FuncMappingWithSync<TFrom, TTo>(this.aMappingFunc, this.aMappingAction);
			else 
				return new FuncMappingWithMap<TFrom, TTo>(this.aMappingFunc);
				
		}

		#endregion
	}
}
