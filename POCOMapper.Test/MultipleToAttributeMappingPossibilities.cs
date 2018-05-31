using KST.POCOMapper.definition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.POCOMapper.Test
{
	[TestClass]
	public class MultipleToAttributeMappingPossibilities
	{
		private class From
		{
			public string DataData = "hello world";
		}

		private class ToInner
		{
			public string Data;
		}

		private class ToSuper
		{
			public ToInner Data;
		}

		private class To : ToSuper
		{
			public string DataData;
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
			}
		}

		[TestMethod]
		public void StructuringMapTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
			Assert.AreEqual(null, ret.Data);
			Assert.AreEqual("hello world", ret.DataData);
		}
	}
}
