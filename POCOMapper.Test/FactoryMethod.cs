using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.POCOMapper.Test
{
	[TestClass]
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

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.ObjectMappingRules()
					.Factory(from => new To(5));
			}
		}

		[TestMethod]
		public void MapppingTest()
		{
			From from = new From();

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual(5, to.a);
		}
	}
}
