using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.typePatterns;

namespace POCOMapper.Test
{
	[TestClass]
	public class PatternMatching
	{
		private interface ITest
		{

		}

		private class Test1
		{

		}

		private class Test2 : Test1, ITest
		{

		}

		private class GenericTest<T> where T : Test1
		{

		}

		[TestMethod]
		public void MatchesToAny()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "?");
			Assert.IsTrue(pattern.Matches(typeof(Test1)));
		}

		[TestMethod]
		public void MatchesToItSelf()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "POCOMapper.Test.PatternMatching+Test1");
			Assert.IsTrue(pattern.Matches(typeof(Test1)));
		}

		[TestMethod]
		public void MatchesToItSelfAsBase()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "? extends POCOMapper.Test.PatternMatching+Test1");
			Assert.IsTrue(pattern.Matches(typeof(Test1)));
		}

		[TestMethod]
		public void InvalidMatch()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "POCOMapper.Test.PatternMatching+Test1");
			Assert.IsFalse(pattern.Matches(typeof(Test2)));
		}

		[TestMethod]
		public void MatchesToItsBase()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "? extends POCOMapper.Test.PatternMatching+Test1");
			Assert.IsTrue(pattern.Matches(typeof(Test2)));
		}

		[TestMethod]
		public void GenericMatchesToItSelf()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "POCOMapper.Test.PatternMatching+GenericTest<?>");
			Assert.IsTrue(pattern.Matches(typeof(GenericTest<Test1>)));
		}

		[TestMethod]
		public void ExactGenericDefinitionMatchesToItSelf()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "POCOMapper.Test.PatternMatching+GenericTest<POCOMapper.Test.PatternMatching+Test1>");
			Assert.IsTrue(pattern.Matches(typeof(GenericTest<Test1>)));
		}

		[TestMethod]
		public void GenericMatchesToItsBase()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "? extends POCOMapper.Test.PatternMatching+GenericTest<?>");
			Assert.IsTrue(pattern.Matches(typeof(GenericTest<Test1>)));
		}

		[TestMethod]
		public void ExactGenericDefinitionMatchesToItSelfWithBase()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "POCOMapper.Test.PatternMatching+GenericTest<? extends POCOMapper.Test.PatternMatching+Test1>");
			Assert.IsTrue(pattern.Matches(typeof(GenericTest<Test1>)));
		}
	}
}
