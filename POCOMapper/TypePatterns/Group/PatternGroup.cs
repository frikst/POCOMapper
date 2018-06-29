using System;
using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Executor;

namespace KST.POCOMapper.TypePatterns.Group
{
	public class PatternGroup
    {
	    private readonly IPattern[] aPatterns;
	    private readonly List<PatternGroupWhereCondition> aWhereConditions;

	    public PatternGroup(IEnumerable<IPattern> patterns)
	    {
		    this.aPatterns = patterns.ToArray();
			this.aWhereConditions = new List<PatternGroupWhereCondition>();
	    }

	    public PatternGroup(params IPattern[] patterns)
			: this(patterns.AsEnumerable())
	    { }

	    public void AddWhereCondition(PatternGroupWhereCondition whereCondition)
	    {
			this.aWhereConditions.Add(whereCondition);
	    }

	    public bool Matches(MappingDefinitionInformation mappingDefinitionInformation, IEnumerable<Type> types)
	    {
			var typeChecker = new TypeChecker();
		    var typeArray = types as ICollection<Type> ?? types.ToArray();

			if (typeArray.Count != this.aPatterns.Length)
				throw new ArgumentException($"Pattern group should be compared to exactly {this.aPatterns.Length} types.");

		    foreach (var (type, pattern) in typeArray.Zip(this.aPatterns, (type, pattern) => (type, pattern)))
		    {
			    if (!pattern.Matches(type, typeChecker))
				    return false;
		    }

		    var patternWhereEvaluator = new PatternWhereEvaluator(typeChecker, mappingDefinitionInformation);

		    foreach (var whereCondition in this.aWhereConditions)
		    {
			    if (!whereCondition(patternWhereEvaluator))
				    return false;
		    }

		    return true;
	    }

	    public bool Matches(IEnumerable<Type> types)
		    => this.Matches(null, types);

	    public bool Matches(MappingDefinitionInformation mappingDefinitionInformation, params Type[] types)
		    => this.Matches(mappingDefinitionInformation, types.AsEnumerable());

	    public bool Matches(params Type[] types)
		    => this.Matches(types.AsEnumerable());
    }
}
