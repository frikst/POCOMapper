using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Visitor;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class ExplicitMappingPrecedence
	{
		private class From
		{
			public string attr = "incorrect";
			public string attrSrc = "correct";
		}

		private class To
		{
			public string attr;
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.ObjectMappingRules()
					.Member<string, string>("attrSrc", "attr", def => { });
			}
		}

		[Test]
		public void MapppingTest()
		{
			From from = new From();

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual("correct", to.attr);
		}

		[Test]
		public void ToStringTest()
		{
			string correct = "ObjectToObject<From, To>\n    attrSrc => attr (null)\n" + Constants.STANDARD_MAPPINGS;

			ToStringVisitor visitor = new ToStringVisitor();

			Mapping.Instance.AcceptForAll(visitor);

			string mappingToString = visitor.GetResult();

			Assert.AreEqual(correct, mappingToString);
		}
	}
}
