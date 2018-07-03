using KST.POCOMapper.Executor.TypeMappingDefinition;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.TypePatterns;
using KST.POCOMapper.TypePatterns.Group;

namespace KST.POCOMapper.Definition.TypeMappingDefinition
{
	/// <summary>
	/// Pattern mapping specification definition class.
	/// </summary>
	public class PatternTypeMappingDefinitionBuilder : ITypeMappingDefinitionBuilder, IRulesDefinition
	{
		private readonly PatternGroup aPatterns;

		private int aPriority;
		private IMappingRules aRules;

		internal PatternTypeMappingDefinitionBuilder(IPattern patternFrom, IPattern patternTo)
		{
			this.aPatterns = new PatternGroup(patternFrom, patternTo);
			this.aPriority = 0;
			this.aRules = null;
		}

		#region Implementation of ITypeMappingDefinition

		ITypeMappingDefinition ITypeMappingDefinitionBuilder.Finish()
		{
			return new PatternTypeMappingDefinition(this.aPatterns, this.aPriority, this.aRules);
		}

		#endregion

		public PatternTypeMappingDefinitionBuilder Where(PatternGroupWhereCondition whereCondition)
		{
			this.aPatterns.AddWhereCondition(whereCondition);

			return this;
		}

		public PatternTypeMappingDefinitionBuilder SetPriority(int priority)
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
