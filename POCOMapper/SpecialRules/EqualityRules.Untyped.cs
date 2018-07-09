using System;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Object;

namespace KST.POCOMapper.SpecialRules
{
	public class EqualityRules : IMappingRules, IRulesDefinition, IEqualityRules
	{
		private IMappingRules aRules;
		private Delegate aIdFrom;
		private Delegate aIdTo;

		public EqualityRules()
		{
			this.aRules = new ObjectMappingRules();
		}

		public void Id(Func<dynamic, dynamic> idFrom, Func<dynamic, dynamic> idTo)
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

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingDefinitionInformation mappingDefinition)
		{
			return this.aRules.Create<TFrom, TTo>(mappingDefinition);
		}

		#endregion

		#region Implementation of IRulesDefinition

		public TRules Rules<TRules>()
			where TRules : class, IMappingRules, new()
		{
			TRules ret = new TRules();
			this.aRules = ret;
			return ret;
		}

		#endregion
	}
}
