using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Common;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class ImplicitAndExplicitMapping
	{

		private class From
		{
			public int Id = 5;
			public int Inner = 2;
		}

		private class ToInner
		{
			public int Id;
		}

		private class To
		{
			public int Id;
			public ToInner Inner = new ToInner();
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.ObjectMappingRules()
					.Member("Inner", "Inner.Id");
			}
		}

		[Test]
		public void TestMethod1()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
			Assert.AreEqual(5, ret.Id);
			Assert.AreEqual(2, ret.Inner.Id);
		}
	}
}
