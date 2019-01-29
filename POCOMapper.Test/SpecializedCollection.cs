using System.Collections.Generic;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Validation;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
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

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
			}
		}

		[Test]
		public void SpecializedToGenericMapping()
		{
			List<int> ret = Mapping.Instance.Map<ListOfInts, List<int>>(new ListOfInts { 1, 2, 3 });

			Assert.AreEqual(3, ret.Count);
			Assert.AreEqual(1, ret[0]);
			Assert.AreEqual(2, ret[1]);
			Assert.AreEqual(3, ret[2]);
		}

		[Test]
		public void GenericToSpecializedMapping()
		{
			ListOfInts ret = Mapping.Instance.Map<List<int>, ListOfInts>(new List<int> { 1, 2, 3 });

			Assert.AreEqual(3, ret.Count);
			Assert.AreEqual(1, ret[0]);
			Assert.AreEqual(2, ret[1]);
			Assert.AreEqual(3, ret[2]);
		}

		[Test]
		public void ValidateMapping()
		{
			Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
