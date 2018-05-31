using KST.POCOMapper.definition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.POCOMapper.Test
{
	[TestClass]
	public class TwoAttributesWithConflictingPrefix
	{
		private class FromChild
		{
			public int id;
		}

		private class From
		{
			public FromChild prefix;
		}

		private class To
		{
			public int prefixId;
			public int prefixChild;
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
			}
		}

		[TestMethod]
		public void MappingTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From { prefix = new FromChild { id = 5 } });
			Assert.AreEqual(5, ret.prefixId);
		}
	}
}
