using KST.POCOMapper.Definition;
using KST.POCOMapper.Visitor;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class Structuring
	{
		private class From
		{
			public string InnerData = "hello";
			public string Data = "world";
		}

		private class ToInner
		{
			public string Data;
		}

		private class To
		{
			public ToInner Inner;
			public string Data;
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
			}
		}

		[Test]
		public void StructuringMappingTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
			Assert.AreEqual("hello", ret.Inner.Data);
			Assert.AreEqual("world", ret.Data);
		}

		[Test]
		public void StructuringSynchronizationTest()
		{
			To to = new To();
			From from = new From();

			Mapping.Instance.Synchronize(from, ref to);

			Assert.AreEqual("hello", to.Inner.Data);
			Assert.AreEqual("world", to.Data);
		}

		[Test]
		public void StructuringToStringTest()
		{
			string correct = "ObjectToObject<From, To>\n    InnerData => Inner.Data Copy<String>\n    Data => Data Copy<String>";

			ToStringVisitor visitor = new ToStringVisitor();

			Mapping.Instance.Mappings.AcceptForAll(visitor);

			string mappingToString = visitor.GetResult();

			Assert.AreEqual(correct, mappingToString);
		}
	}
}