using System;
using System.Collections.Generic;
using System.Text;
using KST.POCOMapper.TypePatterns;
using KST.POCOMapper.TypePatterns.DefinitionHelpers;
using KST.POCOMapper.TypePatterns.Group;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
    public class PatternGroupMatching
    {
		private class GAny : GenericParameter { }

	    private class GFrom : GenericParameter { }
	    private class GTo : GenericParameter { }

	    [Test]
	    public void SameParameter()
	    {
		    var pattern = new PatternGroup(new Pattern<IEnumerable<GAny>>(), new Pattern<GAny>());

		    Assert.IsTrue(pattern.Matches(typeof(IEnumerable<int>), typeof(int)));
		    Assert.IsFalse(pattern.Matches(typeof(IEnumerable<int>), typeof(bool)));
		    Assert.IsFalse(pattern.Matches(typeof(int), typeof(IEnumerable<int>)));
	    }

	    [Test]
	    public void DifferentParameters()
	    {
		    var pattern = new PatternGroup(new Pattern<IEnumerable<GFrom>>(), new Pattern<GTo>());

		    Assert.IsTrue(pattern.Matches(typeof(IEnumerable<int>), typeof(int)));
		    Assert.IsTrue(pattern.Matches(typeof(IEnumerable<int>), typeof(bool)));
		    Assert.IsFalse(pattern.Matches(typeof(int), typeof(IEnumerable<int>)));
	    }

	    [Test]
	    public void WhereConditionIsPrimitive()
	    {
		    var pattern = new PatternGroup(new Pattern<IEnumerable<GFrom>>(), new Pattern<GTo>());
			pattern.AddWhereCondition(x => x.IsPrimitive<GFrom>() && x.IsPrimitive<GTo>());

		    Assert.IsTrue(pattern.Matches(typeof(IEnumerable<int>), typeof(bool)));
		    Assert.IsFalse(pattern.Matches(typeof(IEnumerable<int>), typeof(object)));
		    Assert.IsFalse(pattern.Matches(typeof(IEnumerable<object>), typeof(int)));
	    }

	    [Test]
	    public void WhereConditionIsCastable()
	    {
		    var pattern = new PatternGroup(new Pattern<IEnumerable<GFrom>>(), new Pattern<GTo>());
		    pattern.AddWhereCondition(x => x.IsImplicitlyCastable<GFrom, GTo>());

		    Assert.IsFalse(pattern.Matches(typeof(IEnumerable<int>), typeof(bool)));
		    Assert.IsFalse(pattern.Matches(typeof(IEnumerable<int>), typeof(object)));
		    Assert.IsTrue(pattern.Matches(typeof(IEnumerable<int>), typeof(double)));
	    }
    }
}
