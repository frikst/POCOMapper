using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Decorators;
using KST.POCOMapper.Validation;
using KST.POCOMapper.Visitor;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class RecursiveStructure
	{
		private class From
		{
			public From Inner;
		}

		private class To
		{
			public To Inner;
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.NullableRules();
			}
		}

		[Test]
		public void MapEmpty()
		{
			From from = new From { };

			To to = Mapping.Instance.Map<From, To>(from);

			Assert.IsNotNull(to);
			Assert.IsNull(to.Inner);
		}

		[Test]
		public void MapOneLevel()
		{
			From from = new From { Inner = new From { } };

			To to = Mapping.Instance.Map<From, To>(from);

			Assert.IsNotNull(to);
			Assert.IsNotNull(to.Inner);
			Assert.IsNull(to.Inner.Inner);
		}

		[Test]
		public void MapTwoLevels()
		{
			From from = new From { Inner = new From { Inner = new From { } } };

			To to = Mapping.Instance.Map<From, To>(from);

			Assert.IsNotNull(to);
			Assert.IsNotNull(to.Inner);
			Assert.IsNotNull(to.Inner.Inner);
			Assert.IsNull(to.Inner.Inner.Inner);
		}

		[Test]
		public void MappingToString()
		{
			string expected = "NullableWithSync<From, To>\n    ObjectToObject<From, To>\n        Inner => Inner NullableWithSync<From, To>\n            ...";

			ToStringVisitor visitor = new ToStringVisitor();

			Mapping.Instance.Mappings.AcceptForAll(visitor);

			string mappingToString = visitor.GetResult();

			Assert.AreEqual(expected, mappingToString);
		}

		[Test]
		public void ValidateMapping()
		{
			Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
