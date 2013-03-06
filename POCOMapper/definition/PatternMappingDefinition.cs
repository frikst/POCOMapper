using System;
using System.Reflection;
using POCOMapper.mapping.@base;
using POCOMapper.typePatterns;

namespace POCOMapper.definition
{
	/// <summary>
	/// Pattern mapping specification definition class.
	/// </summary>
	public class PatternMappingDefinition : IMappingDefinition, IRulesDefinition
	{
		private readonly IPattern aPatternFrom;
		private readonly IPattern aPatternTo;

		private int aPriority;
		private IMappingRules aRules;


		internal PatternMappingDefinition(IPattern patternFrom, IPattern patternTo)
		{
			this.aPatternFrom = patternFrom;
			this.aPatternTo = patternTo;
			this.aPriority = 0;
			this.aRules = null;
		}

		#region Implementation of IMappingDefinition

		IMapping IMappingDefinition.CreateMapping(MappingImplementation allMappings, Type from, Type to)
		{
			MethodInfo mappingCreateMethod = typeof(IMappingRules).GetMethod("Create").MakeGenericMethod(from, to);
			return (IMapping) mappingCreateMethod.Invoke(this.aRules, new object[] { allMappings });
		}

		bool IMappingDefinition.IsFrom(Type from)
		{
			return this.aPatternFrom.Matches(from);
		}

		bool IMappingDefinition.IsTo(Type to)
		{
			return this.aPatternTo.Matches(to);
		}

		Tuple<Type, Type> IMappingDefinition.GetKey()
		{
			return null;
		}

		int IMappingDefinition.Priority
		{
			get { return this.aPriority; }
		}

		#endregion

		public PatternMappingDefinition SetPriority(int priority)
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
	}
}
