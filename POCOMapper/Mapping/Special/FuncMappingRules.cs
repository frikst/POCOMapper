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

		public void Using(Action<TFrom, TTo> mappingAction)
		{
			this.aMappingAction = mappingAction;
		}

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingDefinitionInformation mappingDefinition)
		{
			if (this.aMappingFunc != null)
				return new FuncMapping<TFrom, TTo>(this.aMappingFunc);
			else if (this.aMappingAction != null)
				return new FuncMapping<TFrom, TTo>(this.aMappingAction);
			else
				throw new InvalidMappingException("Function mapping without the mapping function defined");
		}

		#endregion
	}
}
