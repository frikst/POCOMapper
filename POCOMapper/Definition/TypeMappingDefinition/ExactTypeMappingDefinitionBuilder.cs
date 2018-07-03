using KST.POCOMapper.Executor.TypeMappingDefinition;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Definition.TypeMappingDefinition
{
	/// <summary>
	/// Class mapping specification definition class.
	/// </summary>
	/// <typeparam name="TFrom">Source class.</typeparam>
	/// <typeparam name="TTo">Destination class.</typeparam>
	public class ExactTypeMappingDefinitionBuilder<TFrom, TTo> : ITypeMappingDefinitionBuilder, IRulesDefinition<TFrom, TTo>
	{
		private int aPriority;
		private IMappingRules<TFrom, TTo> aRules;
		private bool aVisitable;

		internal ExactTypeMappingDefinitionBuilder()
		{
			this.aPriority = 0;
			this.aVisitable = true;
			this.aRules = new ObjectMappingRules<TFrom, TTo>();
		}

		#region Implementation of ITypeMappingDefinitionBuilder

		ITypeMappingDefinition ITypeMappingDefinitionBuilder.Finish()
		{
			return new ExactTypeMappingDefinition<TFrom, TTo>(this.aPriority, this.aVisitable, this.aRules);
		}

		#endregion

		public ExactTypeMappingDefinitionBuilder<TFrom, TTo> SetPriority(int priority)
		{
			this.aPriority = priority;

			return this;
		}

		public ExactTypeMappingDefinitionBuilder<TFrom, TTo> NotVisitable
		{
			get
			{
				this.aVisitable = false;

				return this;
			}
		}

		#region Implementation of IRulesDefinition<TFrom,TTo>

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