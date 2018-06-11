using KST.POCOMapper.TypePatterns;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
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

		[Test]
		public void MatchesToAny()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "?");
			Assert.IsTrue(pattern.Matches(typeof(Test1)));
		}

		[Test]
		public void MatchesToItSelf()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "KST.POCOMapper.Test.PatternMatching+Test1");
			Assert.IsTrue(pattern.Matches(typeof(Test1)));
		}

		[Test]
		public void MatchesToItSelfAsBase()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "? extends KST.POCOMapper.Test.PatternMatching+Test1");
			Assert.IsTrue(pattern.Matches(typeof(Test1)));
		}

		[Test]
		public void InvalidMatch()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "KST.POCOMapper.Test.PatternMatching+Test1");
			Assert.IsFalse(pattern.Matches(typeof(Test2)));
		}

		[Test]
		public void MatchesToItsBase()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "? extends KST.POCOMapper.Test.PatternMatching+Test1");
			Assert.IsTrue(pattern.Matches(typeof(Test2)));
		}

		[Test]
		public void GenericMatchesToItSelf()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "KST.POCOMapper.Test.PatternMatching+GenericTest<?>");
			Assert.IsTrue(pattern.Matches(typeof(GenericTest<Test1>)));
		}

		[Test]
		public void ExactGenericDefinitionMatchesToItSelf()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "KST.POCOMapper.Test.PatternMatching+GenericTest<KST.POCOMapper.Test.PatternMatching+Test1>");
			Assert.IsTrue(pattern.Matches(typeof(GenericTest<Test1>)));
		}

		[Test]
		public void GenericMatchesToItsBase()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "? extends KST.POCOMapper.Test.PatternMatching+GenericTest<?>");
			Assert.IsTrue(pattern.Matches(typeof(GenericTest<Test1>)));
		}

		[Test]
		public void ExactGenericDefinitionMatchesToItSelfWithBase()
		{
			IPattern pattern = new Pattern(typeof(PatternMatching).Assembly, "KST.POCOMapper.Test.PatternMatching+GenericTest<? extends KST.POCOMapper.Test.PatternMatching+Test1>");
			Assert.IsTrue(pattern.Matches(typeof(GenericTest<Test1>)));
		}
	}
}
