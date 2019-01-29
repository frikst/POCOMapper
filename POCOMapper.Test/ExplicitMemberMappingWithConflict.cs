using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Validation;
using KST.POCOMapper.Visitor;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class ExplicitMemberMappingWithConflict
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

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.ObjectMappingRules()
					.Member<string, string>("attrSrc", "attr", def => { });
			}
		}

		[Test]
		public void ExplicitMemberMappingWithConflictTest()
		{
			From from = new From();

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual("correct", to.attr);
		}

		[Test]
		public void ExplicitMemberMappingWithConflictToStringTest()
		{
			string correct = "ObjectToObject<From, To>\n    attrSrc => attr Copy<String>";

			ToStringVisitor visitor = new ToStringVisitor();

			Mapping.Instance.Mappings.AcceptForAll(visitor);

			string mappingToString = visitor.GetResult();

			Assert.AreEqual(correct, mappingToString);
		}

		[Test]
		public void ValidateMapping()
		{
			Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
