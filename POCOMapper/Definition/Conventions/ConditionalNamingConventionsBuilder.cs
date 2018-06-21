using KST.POCOMapper.Conventions;
using KST.POCOMapper.TypePatterns;

namespace KST.POCOMapper.Definition.Conventions
{
    public class ConditionalNamingConventionsBuilder : NamingConventionsBuilder
    {
	    private readonly IPattern aPatternFrom;
	    private readonly IPattern aPatternTo;

	    internal ConditionalNamingConventionsBuilder(NamingConventions.Direction direction, IPattern patternFrom, IPattern patternTo)
		    : base(direction)
	    {
		    this.aPatternFrom = patternFrom;
		    this.aPatternTo = patternTo;
	    }

	    #region Overrides of NamingConventionsBuilder

	    internal override NamingConventions Finish()
	    {
		    return new ConditionalConventions(
				this.ConventionDirection,
				this.Fields, this.Methods, this.Properties,
				this.MemberScanningPrecedence,
				this.aPatternFrom, this.aPatternTo
			);
	    }

	    #endregion
    }
}
