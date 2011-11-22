using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
	public class ArrayToArray
	{
		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				
			}
		}

		[TestMethod]
		public void TestMethod1()
		{
			int[] ret = Mapping.Instance.Map<int[], int[]>(new int[] {1, 2, 3});
			Assert.AreEqual(ret.Length, 3);
			Assert.AreEqual(ret[0], 1);
			Assert.AreEqual(ret[1], 2);
			Assert.AreEqual(ret[2], 3);
		}
	}
}
