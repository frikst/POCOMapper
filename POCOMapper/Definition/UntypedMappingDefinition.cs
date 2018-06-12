using System;
using System.Reflection;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Common;

namespace KST.POCOMapper.Definition
{
	/// <summary>
	/// Untyped mapping specification definition class.
	/// </summary>
	public class UntypedMappingDefinition : IExactMappingDefinition, IRulesDefinition
	{
		private readonly Type aFrom;
		private readonly Type aTo;

		private int aPriority;
		private IMappingRules aRules;

		internal UntypedMappingDefinition(Type from, Type to)
		{
			this.aFrom = from;
			this.aTo = to;
			this.aRules = new ObjectMappingRules();
		}

		#region Implementation of IMappingDefinition

		IMapping IMappingDefinition.CreateMapping(MappingImplementation allMappings, Type from, Type to)
		{
			MethodInfo mappingCreateMethod = MappingRulesMethods.GetCreate(from, to);
			return (IMapping)mappingCreateMethod.Invoke(this.aRules, new object[] { allMappings });
		}

		bool IMappingDefinition.IsFrom(Type from)
		{
			return this.aFrom == from;
		}

		bool IMappingDefinition.IsTo(Type to)
		{
			return this.aTo == to;
		}

		int IMappingDefinition.Priority
			=> this.aPriority;

		#endregion

		public UntypedMappingDefinition SetPriority(int priority)
		{
			this.aPriority = priority;

			return this;
		}

		#region Implementation of IRulesDefinition

		public TRules Rules<TRules>()
			where TRules : class, IMappingRules, new()
		{
			TRules ret = new TRules();
			this.aRules = ret;
			return ret;
		}

		#endregion

		#region Implementation of IExactMappingDefinition

		Type IExactMappingDefinition.From
			=> this.aFrom;

		Type IExactMappingDefinition.To
			=> this.aTo;

		#endregion
	}
}
