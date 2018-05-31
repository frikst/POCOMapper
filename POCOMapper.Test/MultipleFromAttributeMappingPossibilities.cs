using KST.POCOMapper.definition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.POCOMapper.Test
{
	[TestClass]
	public class MultipleFromAttributeMappingPossibilities
	{
		private class FromInner
		{
			public string Data = "good bye";
		}

		private class FromSuper
		{
			public FromInner Data = new FromInner();
		}

		private class From : FromSuper
		{
			public string DataData = "hello world";
		}

		private class To
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
			Assert.AreEqual("hello world", ret.DataData);
		}
	}
}
