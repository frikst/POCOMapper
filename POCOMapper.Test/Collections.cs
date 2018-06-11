using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Definition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.POCOMapper.Test
{
	[TestClass]
	public class Collections
	{
		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				
			}
		}

		[TestMethod]
		public void ToArray()
		{
			int[] ret = Mapping.Instance.Map<int[], int[]>(new int[] {1, 2, 3});
			Assert.AreEqual(ret.Length, 3);
			Assert.AreEqual(ret[0], 1);
			Assert.AreEqual(ret[1], 2);
			Assert.AreEqual(ret[2], 3);
		}

		[TestMethod]
		public void ToList()
		{
			List<int> ret = Mapping.Instance.Map<int[], List<int>>(new int[] { 1, 2, 3 });
			Assert.AreEqual(ret.Count, 3);
			Assert.AreEqual(ret[0], 1);
			Assert.AreEqual(ret[1], 2);
			Assert.AreEqual(ret[2], 3);
		}

		[TestMethod]
		public void ToHashSet()
		{
			HashSet<int> ret = Mapping.Instance.Map<int[], HashSet<int>>(new int[] { 1, 2, 3 });
			Assert.AreEqual(ret.Count, 3);
			Assert.IsTrue(ret.Contains(1));
			Assert.IsTrue(ret.Contains(2));
			Assert.IsTrue(ret.Contains(3));
		}

		[TestMethod]
		public void ToIEnumerable()
		{
			IEnumerable<int> ret = Mapping.Instance.Map<int[], IEnumerable<int>>(new int[] { 1, 2, 3 });
			Assert.AreEqual(ret.Count(), 3);
			Assert.IsTrue(ret.Contains(1));
			Assert.IsTrue(ret.Contains(2));
			Assert.IsTrue(ret.Contains(3));
		}
	}
}
