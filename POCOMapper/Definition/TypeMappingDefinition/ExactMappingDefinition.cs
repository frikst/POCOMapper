using System;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Definition.TypeMappingDefinition
{
	/// <summary>
	/// Class mapping specification definition class.
	/// </summary>
	/// <typeparam name="TFrom">Source class.</typeparam>
	/// <typeparam name="TTo">Destination class.</typeparam>
	public class ExactTypeMappingDefinition<TFrom, TTo> : IExactTypeMappingDefinition, IRulesDefinition<TFrom, TTo>
	{
		private int aPriority;
		private IMappingRules<TFrom, TTo> aRules;
		private bool aVisitable;

		internal ExactTypeMappingDefinition()
		{
			this.aPriority = 0;
			this.aVisitable = true;
			this.aRules = new ObjectMappingRules<TFrom, TTo>();
		}

		#region Implementation of ITypeMappingDefinition

		IMapping ITypeMappingDefinition.CreateMapping(MappingDefinitionInformation mappingDefinition, Type from, Type to)
		{
			return this.aRules.Create(mappingDefinition);
		}

		bool ITypeMappingDefinition.IsFrom(Type from)
		{
			return from == typeof(TFrom);
		}

		bool ITypeMappingDefinition.IsTo(Type to)
		{
			return to == typeof(TTo);
		}

		int ITypeMappingDefinition.Priority
			=> this.aPriority;

		bool ITypeMappingDefinition.Visitable
			=> this.aVisitable;

		#endregion

		public ExactTypeMappingDefinition<TFrom, TTo> SetPriority(int priority)
		{
			this.aPriority = priority;

			return this;
		}

		public ExactTypeMappingDefinition<TFrom, TTo> NotVisitable
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

		#region Implementation of IExactTypeMappingDefinition

		public Type From
			=> typeof(TFrom);

		public Type To
			=> typeof(TTo);

		#endregion
	}
}