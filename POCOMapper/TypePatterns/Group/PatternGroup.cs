using System;
using System.Collections.Generic;
using System.Linq;

namespace KST.POCOMapper.TypePatterns.Group
{
    public class PatternGroup
    {
	    private readonly IPattern[] aPatterns;

	    public PatternGroup(IEnumerable<IPattern> patterns)
	    {
		    this.aPatterns = patterns.ToArray();
	    }

	    public PatternGroup(params IPattern[] patterns)
			: this(patterns.AsEnumerable())
	    { }

	    public bool Matches(IEnumerable<Type> types)
	    {
			TypeChecker typeChecker = new TypeChecker();
		    var typeArray = types as ICollection<Type> ?? types.ToArray();

			if (typeArray.Count != this.aPatterns.Length)
				throw new ArgumentException($"Pattern group should be compared to exactly {this.aPatterns.Length} types.");

		    return typeArray
			    .Zip(this.aPatterns, (type, pattern) => pattern.Matches(type, typeChecker))
			    .All(x => x);
	    }

	    public bool Matches(params Type[] types)
	    {
		    return this.Matches(types.AsEnumerable());
	    }
    }
}
