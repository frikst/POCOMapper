using System;
using System.Reflection;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.TypePatterns;

namespace KST.POCOMapper.Definition.TypeMappingDefinition
{
	/// <summary>
	/// Pattern mapping specification definition class.
	/// </summary>
	public class PatternTypeMappingDefinition : ITypeMappingDefinition, IRulesDefinition
	{
		private readonly IPattern aPatternFrom;
		private readonly IPattern aPatternTo;

		private int aPriority;
		private IMappingRules aRules;

		internal PatternTypeMappingDefinition(IPattern patternFrom, IPattern patternTo)
		{
			this.aPatternFrom = patternFrom;
			this.aPatternTo = patternTo;
			this.aPriority = 0;
			this.aRules = null;
		}

		#region Implementation of ITypeMappingDefinition

		IMapping ITypeMappingDefinition.CreateMapping(MappingDefinitionInformation mappingDefinition, Type @from, Type to)
		{
			MethodInfo mappingCreateMethod = MappingRulesMethods.GetCreate(from, to);
			return (IMapping) mappingCreateMethod.Invoke(this.aRules, new object[] { mappingDefinition });
		}

		bool ITypeMappingDefinition.IsFrom(Type from)
		{
			return this.aPatternFrom.Matches(from);
		}

		bool ITypeMappingDefinition.IsTo(Type to)
		{
			return this.aPatternTo.Matches(to);
		}
		
		int ITypeMappingDefinition.Priority
			=> this.aPriority;

		bool ITypeMappingDefinition.Visitable
			=> false;

		#endregion

		public PatternTypeMappingDefinition SetPriority(int priority)
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
