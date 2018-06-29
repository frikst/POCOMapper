using KST.POCOMapper.Conventions;
using KST.POCOMapper.TypePatterns;
using KST.POCOMapper.TypePatterns.Group;

namespace KST.POCOMapper.Definition.Conventions
{
    public class ConditionalNamingConventionsBuilder : NamingConventionsBuilder
    {
	    private readonly PatternGroup aTypePatterns;

	    internal ConditionalNamingConventionsBuilder(NamingConventions.Direction direction, PatternGroup typePatterns)
		    : base(direction)
	    {
		    this.aTypePatterns = typePatterns;
	    }

	    public ConditionalNamingConventionsBuilder Where(PatternGroupWhereCondition whereCondition)
	    {
			this.aTypePatterns.AddWhereCondition(whereCondition);

		    return this;
	    }

	    #region Overrides of NamingConventionsBuilder

	    internal override NamingConventions Finish()
	    {
		    return new ConditionalConventions(
				this.ConventionDirection,
				this.Fields, this.Methods, this.Properties,
				this.MemberScanningPrecedence,
				this.aTypePatterns
			);
	    }

	    #endregion
    }
}
