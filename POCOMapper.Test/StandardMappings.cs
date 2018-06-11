using KST.POCOMapper.Definition;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class StandardMappings
	{
		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				
			}
		}

		[Test]
		public void DoubleToInt()
		{
			int ret = Mapping.Instance.Map<double, int>(1.5);
			Assert.AreEqual(1, ret);
		}

		[Test]
		public void IntToDouble()
		{
			double ret = Mapping.Instance.Map<int, double>(1);
			Assert.AreEqual(1.0, ret);
		}

		[Test]
		public void StringToInt()
		{
			int ret = Mapping.Instance.Map<string, int>("158");
			Assert.AreEqual(158, ret);
		}

		[Test]
		public void IntToString()
		{
			string ret = Mapping.Instance.Map<int, string>(158);
			Assert.AreEqual("158", ret);
		}
	}
}
