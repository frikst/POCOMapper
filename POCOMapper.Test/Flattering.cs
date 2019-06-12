using KST.POCOMapper.Definition;
using KST.POCOMapper.Validation;
using KST.POCOMapper.Visitor;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
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

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
			}
		}

		[Test]
		public void FlatteringMapTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
			Assert.AreEqual("hello", ret.InnerData);
			Assert.AreEqual("world", ret.Data);
		}

		[Test]
		public void FlatteringSynchronizationTest()
		{
			To to = new To();
			From from = new From();

			Mapping.Instance.Synchronize(from, ref to);

			Assert.AreEqual("hello", to.InnerData);
			Assert.AreEqual("world", to.Data);
		}

		[Test]
		public void MapEqualTest()
		{
			From from = new From();
            To to = new To {InnerData = "hello", Data = "world"};

            var same = Mapping.Instance.MapEqual(from, to);

            Assert.IsTrue(same);
		}

		[Test]
		public void MapEqualDifferentUnflatteredTest()
		{
			From from = new From();
            To to = new To {InnerData = "hello", Data = "boo"};

            var same = Mapping.Instance.MapEqual(from, to);

            Assert.IsFalse(same);
		}

		[Test]
		public void MapEqualDifferentFlatteredTest()
		{
			From from = new From();
            To to = new To {InnerData = "boo", Data = "world"};

            var same = Mapping.Instance.MapEqual(from, to);

            Assert.IsFalse(same);
		}

		[Test]
		public void FlatteringToStringTest()
		{
			string correct = "ObjectToObject<From, To>\n    Inner.Data => InnerData Copy<String>\n    Data => Data Copy<String>";

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
