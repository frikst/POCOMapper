using System;
using System.Reflection;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.TypePatterns;
using KST.POCOMapper.TypePatterns.Group;

namespace KST.POCOMapper.Definition.TypeMappingDefinition
{
	/// <summary>
	/// Pattern mapping specification definition class.
	/// </summary>
	public class PatternTypeMappingDefinition : ITypeMappingDefinition, IRulesDefinition
	{
		private readonly PatternGroup aPatterns;

		private int aPriority;
		private IMappingRules aRules;

		internal PatternTypeMappingDefinition(IPattern patternFrom, IPattern patternTo)
		{
			this.aPatterns = new PatternGroup(patternFrom, patternTo);
			this.aPriority = 0;
			this.aRules = null;
		}

		#region Implementation of ITypeMappingDefinition

		IMapping ITypeMappingDefinition.CreateMapping(MappingDefinitionInformation mappingDefinition, Type from, Type to)
		{
			if (!this.aPatterns.Matches(mappingDefinition, from, to))
				throw new InvalidOperationException($"{from.Name} and {to.Name} does not match required patterns");

			MethodInfo mappingCreateMethod = MappingRulesMethods.GetCreate(from, to);
			return (IMapping) mappingCreateMethod.Invoke(this.aRules, new object[] { mappingDefinition });
		}

		bool ITypeMappingDefinition.IsDefinedFor(MappingDefinitionInformation mappingDefinition, Type from, Type to)
		{
			return this.aPatterns.Matches(mappingDefinition, from, to);
		}
		
		int ITypeMappingDefinition.Priority
			=> this.aPriority;

		bool ITypeMappingDefinition.Visitable
			=> false;

		#endregion

		public PatternTypeMappingDefinition Where(PatternGroupWhereCondition whereCondition)
		{
			this.aPatterns.AddWhereCondition(whereCondition);

			return this;
		}

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
