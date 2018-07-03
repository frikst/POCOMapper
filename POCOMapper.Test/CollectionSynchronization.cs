using KST.POCOMapper.Definition;
using KST.POCOMapper.SpecialRules;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class CollectionSynchronization
	{
		private class ItemFrom
		{
			public int id;
			public int value;
		}

		private class ItemTo
		{
			public int id;
			public int value;
			public int anotherValue;
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<ItemFrom, ItemTo>()
					.EqualityRules()
					.Ids(x => x.id, x => x.id);
			}
		}

		[Test]
		public void SimpleSynchronization()
		{
			ItemFrom[] from = new ItemFrom[]
			{
				new ItemFrom { id = 1, value = 5 },
				new ItemFrom { id = 2, value = 4 },
				new ItemFrom { id = 3, value = 3 },
				new ItemFrom { id = 4, value = 2 },
				new ItemFrom { id = 5, value = 1 },
			};

			ItemTo[] to = new ItemTo[]
			{
				new ItemTo { id = 1, value = 10 },
				new ItemTo { id = 2, value = 11 },
				new ItemTo { id = 3, value = 12 },
				new ItemTo { id = 4, value = 13 },
				new ItemTo { id = 5, value = 14 },
			};

			Mapping.Instance.Synchronize(from, ref to);

			Assert.AreEqual(5, to.Length);
			Assert.AreEqual(5, to[0].value);
			Assert.AreEqual(4, to[1].value);
			Assert.AreEqual(3, to[2].value);
			Assert.AreEqual(2, to[3].value);
			Assert.AreEqual(1, to[4].value);
		}

		[Test]
		public void SimpleSynchronizationWithAdditionalData()
		{
			ItemFrom[] from = new ItemFrom[]
			{
				new ItemFrom { id = 1, value = 5 },
				new ItemFrom { id = 2, value = 4 },
				new ItemFrom { id = 3, value = 3 },
				new ItemFrom { id = 4, value = 2 },
				new ItemFrom { id = 5, value = 1 },
			};

			ItemTo[] to = new ItemTo[]
			{
				new ItemTo { id = 1, value = 10, anotherValue = 15 },
				new ItemTo { id = 2, value = 11, anotherValue = 16 },
				new ItemTo { id = 3, value = 12, anotherValue = 17 },
				new ItemTo { id = 4, value = 13, anotherValue = 18 },
				new ItemTo { id = 5, value = 14, anotherValue = 19 },
			};

			Mapping.Instance.Synchronize(from, ref to);

			Assert.AreEqual(5, to.Length);
			Assert.AreEqual(5, to[0].value);
			Assert.AreEqual(15, to[0].anotherValue);
			Assert.AreEqual(4, to[1].value);
			Assert.AreEqual(16, to[1].anotherValue);
			Assert.AreEqual(3, to[2].value);
			Assert.AreEqual(17, to[2].anotherValue);
			Assert.AreEqual(2, to[3].value);
			Assert.AreEqual(18, to[3].anotherValue);
			Assert.AreEqual(1, to[4].value);
			Assert.AreEqual(19, to[4].anotherValue);
		}

		[Test]
		public void SimpleSynchronizationWithAdditionalDataAndItemAddition()
		{
			ItemFrom[] from = new ItemFrom[]
			{
				new ItemFrom { id = 1, value = 5 },
				new ItemFrom { id = 2, value = 4 },
				new ItemFrom { id = 3, value = 3 },
				new ItemFrom { id = 4, value = 2 },
				new ItemFrom { id = 5, value = 1 },
			};

			ItemTo[] to = new ItemTo[]
			{
				new ItemTo { id = 1, value = 10, anotherValue = 15 },
				new ItemTo { id = 2, value = 11, anotherValue = 16 },
				new ItemTo { id = 4, value = 13, anotherValue = 18 },
				new ItemTo { id = 5, value = 14, anotherValue = 19 },
			};

			Mapping.Instance.Synchronize(from, ref to);

			Assert.AreEqual(5, to.Length);
			Assert.AreEqual(5, to[0].value);
			Assert.AreEqual(15, to[0].anotherValue);
			Assert.AreEqual(4, to[1].value);
			Assert.AreEqual(16, to[1].anotherValue);
			Assert.AreEqual(3, to[2].value);
			Assert.AreEqual(0, to[2].anotherValue);
			Assert.AreEqual(2, to[3].value);
			Assert.AreEqual(18, to[3].anotherValue);
			Assert.AreEqual(1, to[4].value);
			Assert.AreEqual(19, to[4].anotherValue);
		}

		[Test]
		public void SimpleSynchronizationWithAdditionalDataAndItemRemoval()
		{
			ItemFrom[] from = new ItemFrom[]
			{
				new ItemFrom { id = 1, value = 5 },
				new ItemFrom { id = 2, value = 4 },
				new ItemFrom { id = 4, value = 2 },
				new ItemFrom { id = 5, value = 1 },
			};

			ItemTo[] to = new ItemTo[]
			{
				new ItemTo { id = 1, value = 10, anotherValue = 15 },
				new ItemTo { id = 2, value = 11, anotherValue = 16 },
				new ItemTo { id = 3, value = 12, anotherValue = 17 },
				new ItemTo { id = 4, value = 13, anotherValue = 18 },
				new ItemTo { id = 5, value = 14, anotherValue = 19 },
			};

			Mapping.Instance.Synchronize(from, ref to);

			Assert.AreEqual(4, to.Length);
			Assert.AreEqual(5, to[0].value);
			Assert.AreEqual(15, to[0].anotherValue);
			Assert.AreEqual(4, to[1].value);
			Assert.AreEqual(16, to[1].anotherValue);
			Assert.AreEqual(2, to[2].value);
			Assert.AreEqual(18, to[2].anotherValue);
			Assert.AreEqual(1, to[3].value);
			Assert.AreEqual(19, to[3].anotherValue);
		}
	}
}
