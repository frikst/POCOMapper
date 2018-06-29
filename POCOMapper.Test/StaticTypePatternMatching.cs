using System.Collections.Generic;
using KST.POCOMapper.TypePatterns;
using KST.POCOMapper.TypePatterns.DefinitionHelpers;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class StaticTypePatternMatching
	{
		private abstract class GAny : GenericParameter { }

		private abstract class GAny2 : GenericParameter { }

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
			IPattern pattern = new Pattern<GAny>();
			Assert.IsTrue(pattern.Matches(typeof(Test1), new TypeChecker()));
		}

		[Test]
		public void MatchesToItSelf()
		{
			IPattern pattern = new Pattern<Test1>();
			Assert.IsTrue(pattern.Matches(typeof(Test1), new TypeChecker()));
		}

		[Test]
		public void MatchesToItSelfAsBase()
		{
			IPattern pattern = new Pattern<SubClass<Test1>>();
			Assert.IsTrue(pattern.Matches(typeof(Test1), new TypeChecker()));
		}

		[Test]
		public void InvalidMatch()
		{
			IPattern pattern = new Pattern<Test1>();
			Assert.IsFalse(pattern.Matches(typeof(Test2), new TypeChecker()));
		}

		[Test]
		public void MatchesToItsBase()
		{
			IPattern pattern = new Pattern<SubClass<Test1>>();
			Assert.IsTrue(pattern.Matches(typeof(Test2), new TypeChecker()));
		}

		[Test]
		public void GenericMatchesToItSelf()
		{
			IPattern pattern = new Pattern<Generic<GenericTest<Test2>>.With<GAny>>();
			Assert.IsTrue(pattern.Matches(typeof(GenericTest<Test1>), new TypeChecker()));
		}

		[Test]
		public void ExactGenericDefinitionMatchesToItSelf()
		{
			IPattern pattern = new Pattern<GenericTest<Test1>>();
			Assert.IsTrue(pattern.Matches(typeof(GenericTest<Test1>), new TypeChecker()));
		}

		[Test]
		public void GenericMatchesToItsBase()
		{
			IPattern pattern = new Pattern<SubClass<Generic<GenericTest<Test2>>.With<GAny>>>();
			Assert.IsTrue(pattern.Matches(typeof(GenericTest<Test1>), new TypeChecker()));
		}

		[Test]
		public void ExactGenericDefinitionMatchesToItSelfWithBase()
		{
			IPattern pattern = new Pattern<SubClass<GenericTest<Test1>>>();
			Assert.IsTrue(pattern.Matches(typeof(GenericTest<Test1>), new TypeChecker()));
		}

		[Test]
		public void DictionaryWithSameKeyValue()
		{
			IPattern pattern = new Pattern<Dictionary<GAny, GAny>>();
			Assert.IsTrue(pattern.Matches(typeof(Dictionary<string, string>), new TypeChecker()));
			Assert.IsFalse(pattern.Matches(typeof(Dictionary<string, int>), new TypeChecker()));
		}

		[Test]
		public void DictionaryWithDifferentKeyAndValue()
		{
			IPattern pattern = new Pattern<Dictionary<GAny, GAny2>>();
			Assert.IsTrue(pattern.Matches(typeof(Dictionary<string, string>), new TypeChecker()));
			Assert.IsTrue(pattern.Matches(typeof(Dictionary<string, int>), new TypeChecker()));
		}
	}
}
