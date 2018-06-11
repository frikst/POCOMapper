using KST.POCOMapper.Definition;
using KST.POCOMapper.Visitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.POCOMapper.Test
{
	[TestClass]
	public class UntypedMapping
	{
		private class From
		{

		}

		private class To
		{

		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map(typeof(From), typeof(To));
			}
		}

		[TestMethod]
		public void CheckCorrectTypeTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
		}

		[TestMethod]
		public void ToStringTest()
		{
			string correct = "ObjectToObject<From, To>\n" + Constants.STANDARD_MAPPINGS;

			ToStringVisitor visitor = new ToStringVisitor();

			Mapping.Instance.AcceptForAll(visitor);

			string mappingToString = visitor.GetResult();

			Assert.AreEqual(correct, mappingToString);
		}
	}
}
