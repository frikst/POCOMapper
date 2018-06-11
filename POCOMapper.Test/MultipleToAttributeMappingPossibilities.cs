using KST.POCOMapper.Definition;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
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

		[Test]
		public void StructuringMapTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
			Assert.AreEqual(null, ret.Data);
			Assert.AreEqual("hello world", ret.DataData);
		}
	}
}
