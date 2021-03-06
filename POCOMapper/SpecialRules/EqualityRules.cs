﻿using System;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Object;

namespace KST.POCOMapper.SpecialRules
{
	public class EqualityRules<TFrom, TTo> : IMappingRules<TFrom, TTo>, IRulesDefinition<TFrom, TTo>, IEqualityRules
	{
		private IMappingRules<TFrom, TTo> aRules;
		private Delegate aIdFrom;
		private Delegate aIdTo;

		public EqualityRules()
		{
			this.aRules = new ObjectMappingRules<TFrom, TTo>();
		}

		public void Ids<TIdType>(Func<TFrom, TIdType> idFrom, Func<TTo, TIdType> idTo)
		{
			this.aIdFrom = idFrom;
			this.aIdTo = idTo;
		}

		#region Implementation of IEqualityRules

		(Delegate IdFrom, Delegate IdTo) IEqualityRules.GetIdSelectors()
		{
			return (this.aIdFrom, this.aIdTo);
		}

		#endregion

		#region Implementation of IMappingRules<TFrom, TTo>

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingDefinitionInformation mappingDefinition)
		{
			return this.aRules.Create(mappingDefinition);
		}

		#endregion

		#region Implementation of IRulesDefinition

		public TRules Rules<TRules>()
			where TRules : class, IMappingRules<TFrom, TTo>, new()
		{
			TRules ret = new TRules();
			this.aRules = ret;
			return ret;
		}

		#endregion
	}
}
