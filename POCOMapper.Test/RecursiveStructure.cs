using KST.POCOMapper.Definition;
using KST.POCOMapper.Visitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.POCOMapper.Test
{
	[TestClass]
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

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
			}
		}

		[TestMethod]
		public void MapEmpty()
		{
			From from = new From { };

			To to = Mapping.Instance.Map<From, To>(from);

			Assert.IsNotNull(to);
			Assert.IsNull(to.Inner);
		}

		[TestMethod]
		public void MapOneLevel()
		{
			From from = new From { Inner = new From { } };

			To to = Mapping.Instance.Map<From, To>(from);

			Assert.IsNotNull(to);
			Assert.IsNotNull(to.Inner);
			Assert.IsNull(to.Inner.Inner);
		}

		[TestMethod]
		public void MapTwoLevels()
		{
			From from = new From { Inner = new From { Inner = new From { } } };

			To to = Mapping.Instance.Map<From, To>(from);

			Assert.IsNotNull(to);
			Assert.IsNotNull(to.Inner);
			Assert.IsNotNull(to.Inner.Inner);
			Assert.IsNull(to.Inner.Inner.Inner);
		}

		[TestMethod]
		public void MappingToString()
		{
			string expected = "ObjectToObject<From, To>\n    Inner => Inner ObjectToObject<From, To>\n        ...\n" + Constants.STANDARD_MAPPINGS;

			ToStringVisitor visitor = new ToStringVisitor();

			Mapping.Instance.AcceptForAll(visitor);

			string mappingToString = visitor.GetResult();

			Assert.AreEqual(expected, mappingToString);
		}
	}
}
