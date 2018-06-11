using System.Collections.Generic;
using KST.POCOMapper.Definition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.POCOMapper.Test
{
	[TestClass]
	public class SpecializedCollection
	{
		private class ListOfInts : List<int>
		{
			public ListOfInts()
			{
			}

			public ListOfInts(IEnumerable<int> collection)
				: base(collection)
			{
			}
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
			}
		}

		[TestMethod]
		public void SpecializedToGenericMapping()
		{
			List<int> ret = Mapping.Instance.Map<ListOfInts, List<int>>(new ListOfInts { 1, 2, 3 });

			Assert.AreEqual(3, ret.Count);
			Assert.AreEqual(1, ret[0]);
			Assert.AreEqual(2, ret[1]);
			Assert.AreEqual(3, ret[2]);
		}

		[TestMethod]
		public void GenericToSpecializedMapping()
		{
			ListOfInts ret = Mapping.Instance.Map<List<int>, ListOfInts>(new List<int> { 1, 2, 3 });

			Assert.AreEqual(3, ret.Count);
			Assert.AreEqual(1, ret[0]);
			Assert.AreEqual(2, ret[1]);
			Assert.AreEqual(3, ret[2]);
		}
	}
}
