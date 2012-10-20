using Microsoft.VisualStudio.TestTools.UnitTesting;

using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
	public class NoAttributesMapping
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
				Map<From, To>();
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
			string correct = "ObjectToObject`2<From, To>\n" + Constants.STANDARD_MAPPINGS;

			string mappingToString = Mapping.Instance.AllMappingsToString();

			Assert.AreEqual(correct, mappingToString);
		}
	}
}
