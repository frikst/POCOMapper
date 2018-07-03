using System;
using KST.POCOMapper.Executor.TypeMappingDefinition;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Object;

namespace KST.POCOMapper.Definition.TypeMappingDefinition
{
	/// <summary>
	/// Untyped mapping specification definition class.
	/// </summary>
	public class UntypedTypeMappingDefinitionBuilder : ITypeMappingDefinitionBuilder, IRulesDefinition
	{
		private readonly Type aFrom;
		private readonly Type aTo;

		private int aPriority;
		private IMappingRules aRules;
		private bool aVisitable;

		internal UntypedTypeMappingDefinitionBuilder(Type from, Type to)
		{
			this.aFrom = from;
			this.aTo = to;
			this.aVisitable = true;
			this.aPriority = 0;
			this.aRules = new ObjectMappingRules();
		}

		#region Implementation of IMappingDefinition

		ITypeMappingDefinition ITypeMappingDefinitionBuilder.Finish()
		{
			return new UntypedTypeMappingDefinition(this.aFrom, this.aTo, this.aPriority, this.aVisitable, this.aRules);
		}

		#endregion

		public UntypedTypeMappingDefinitionBuilder SetPriority(int priority)
		{
			this.aPriority = priority;

			return this;
		}

		public UntypedTypeMappingDefinitionBuilder NotVisitable
		{
			get
			{
				this.aVisitable = false;

				return this;
			}
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
	}
}
