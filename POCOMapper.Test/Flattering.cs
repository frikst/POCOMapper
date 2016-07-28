using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;
using POCOMapper.visitor;

namespace POCOMapper.Test
{
	[TestClass]
	public class Flattering
	{
		private class FromInner
		{
			public string Data = "hello";
		}

		private class From
		{
			public FromInner Inner = new FromInner();
			public string Data = "world";
		}

		private class To
		{
			public string InnerData;
			public string Data;
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
			}
		}

		[TestMethod]
		public void FlatteringMapTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
			Assert.AreEqual("hello", ret.InnerData);
			Assert.AreEqual("world", ret.Data);
		}

		[TestMethod]
		public void FlatteringSynchronizationTest()
		{
			To to = new To();
			From from = new From();

			Mapping.Instance.Synchronize(from, ref to);

			Assert.AreEqual("hello", to.InnerData);
			Assert.AreEqual("world", to.Data);
		}

		[TestMethod]
		public void FlatteringToStringTest()
		{
			string correct = "ObjectToObject<From, To>\n    Inner.Data => InnerData (null)\n    Data => Data (null)\n" + Constants.STANDARD_MAPPINGS;

			ToStringVisitor visitor = new ToStringVisitor();

			Mapping.Instance.AcceptForAll(visitor);

			string mappingToString = visitor.GetResult();

			Assert.AreEqual(correct, mappingToString);
		}
	}
}
