using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Object;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class FactoryMethod
	{
		private class From
		{

		}

		private class To
		{
			public int a;

			public To(int a)
			{
				this.a = a;
			}
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.ObjectMappingRules()
					.Factory(from => new To(5));
			}
		}

		[Test]
		public void MapppingTest()
		{
			From from = new From();

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual(5, to.a);
		}
	}
}
