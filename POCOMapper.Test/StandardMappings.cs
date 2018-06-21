using KST.POCOMapper.Definition;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class StandardMappings
	{
		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				
			}
		}

		[Test]
		public void IntToDouble()
		{
			var ret = Mapping.Instance.Map<int, double>(1);
			Assert.AreEqual(1d, ret);
		}

		[Test]
		public void IntToDecimal()
		{
			var ret = Mapping.Instance.Map<int, decimal>(1);
			Assert.AreEqual(1m, ret);
		}

		[Test]
		public void IntToString()
		{
			var ret = Mapping.Instance.Map<int, string>(158);
			Assert.AreEqual("158", ret);
		}
	}
}
