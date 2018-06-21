using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KST.POCOMapper.Conventions;
using KST.POCOMapper.TypePatterns;

namespace KST.POCOMapper.Definition.Conventions
{
    public class GlobalNamingConventionsBuilder : NamingConventionsBuilder
    {
	    private readonly List<NamingConventionsBuilder> aConditionalConventionList;

	    internal GlobalNamingConventionsBuilder(NamingConventions.Direction direction)
		    : base(direction)
	    {
		    this.aConditionalConventionList = new List<NamingConventionsBuilder>();
	    }

	    public GlobalNamingConventionsBuilder ConditionalConventions<TMemberFrom, TMemberTo>(Action<NamingConventionsBuilder> conventions)
	    {
		    NamingConventionsBuilder conv = new ConditionalNamingConventionsBuilder(this.ConventionDirection, new Pattern<TMemberFrom>(), new Pattern<TMemberTo>());
		    conventions(conv);

		    this.aConditionalConventionList.Add(conv);

		    return this;
	    }

	    public GlobalNamingConventionsBuilder ConditionalConventions(IPattern from, IPattern to, Action<NamingConventionsBuilder> conventions)
	    {
		    NamingConventionsBuilder conv = new ConditionalNamingConventionsBuilder(this.ConventionDirection, from, to);
		    conventions(conv);

		    this.aConditionalConventionList.Add(conv);

		    return this;
	    }

	    #region Overrides of NamingConventionsBuilder

	    internal override NamingConventions Finish()
	    {
		    return new GlobalConventions(
			    this.ConventionDirection,
				this.Fields, this.Methods, this.Properties,
				this.MemberScanningPrecedence,
				this.aConditionalConventionList.Select(x => x.Finish())
			);
	    }

	    #endregion
    }
}
