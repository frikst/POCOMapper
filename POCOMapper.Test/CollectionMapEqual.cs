using System.Collections.Generic;
using KST.POCOMapper.Definition;
using KST.POCOMapper.SpecialRules;
using KST.POCOMapper.Validation;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class CollectionMapEqual
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
		public void EqualCollections()
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
				new ItemTo { id = 1, value = 5 },
				new ItemTo { id = 2, value = 4 },
				new ItemTo { id = 3, value = 3 },
				new ItemTo { id = 4, value = 2 },
				new ItemTo { id = 5, value = 1 },
			};

			var same = Mapping.Instance.MapEqual(from, to);

            Assert.IsTrue(same);
		}

		[Test]
		public void FirstShorter()
		{
			ItemFrom[] from = new ItemFrom[]
			{
				new ItemFrom { id = 1, value = 5 },
				new ItemFrom { id = 2, value = 4 },
				new ItemFrom { id = 3, value = 3 },
				new ItemFrom { id = 4, value = 2 },
			};

			ItemTo[] to = new ItemTo[]
			{
				new ItemTo { id = 1, value = 5 },
				new ItemTo { id = 2, value = 4 },
				new ItemTo { id = 3, value = 3 },
				new ItemTo { id = 4, value = 2 },
				new ItemTo { id = 5, value = 1 },
			};

			var same = Mapping.Instance.MapEqual(from, to);

            Assert.IsFalse(same);
		}

        [Test]
        public void SecondShorter()
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
                new ItemTo { id = 1, value = 5 },
                new ItemTo { id = 2, value = 4 },
                new ItemTo { id = 3, value = 3 },
                new ItemTo { id = 4, value = 2 },
            };

            var same = Mapping.Instance.MapEqual(from, to);

            Assert.IsFalse(same);
        }

        [Test]
        public void DifferentIDs()
        {
            ItemFrom[] from = new ItemFrom[]
            {
                new ItemFrom { id = 0, value = 5 },
                new ItemFrom { id = 2, value = 4 },
                new ItemFrom { id = 3, value = 3 },
                new ItemFrom { id = 4, value = 2 },
                new ItemFrom { id = 5, value = 1 },
            };

            ItemTo[] to = new ItemTo[]
            {
                new ItemTo { id = 1, value = 5 },
                new ItemTo { id = 2, value = 4 },
                new ItemTo { id = 3, value = 3 },
                new ItemTo { id = 4, value = 2 },
                new ItemTo { id = 5, value = 1 },
            };

            var same = Mapping.Instance.MapEqual(from, to);

            Assert.IsFalse(same);
        }

        [Test]
        public void DifferentValues()
        {
            ItemFrom[] from = new ItemFrom[]
            {
                new ItemFrom { id = 1, value = 1 },
                new ItemFrom { id = 2, value = 2 },
                new ItemFrom { id = 3, value = 3 },
                new ItemFrom { id = 4, value = 4 },
                new ItemFrom { id = 5, value = 5 },
            };

            ItemTo[] to = new ItemTo[]
            {
                new ItemTo { id = 1, value = 5 },
                new ItemTo { id = 2, value = 4 },
                new ItemTo { id = 3, value = 3 },
                new ItemTo { id = 4, value = 2 },
                new ItemTo { id = 5, value = 1 },
            };

            var same = Mapping.Instance.MapEqual(from, to);

            Assert.IsFalse(same);
        }

        [Test]
        public void ArraysWithSameValues()
        {
            int[] from = new int[] {1, 2, 3};
            int[] to = new int[] {1, 2, 3};

            var same = Mapping.Instance.MapEqual(from, to);

            Assert.IsTrue(same);
        }

        [Test]
        public void ArraysWithDifferentValues()
        {
            int[] from = new int[] {1, 2, 3};
            int[] to = new int[] {1, 2, 5};

            var same = Mapping.Instance.MapEqual(from, to);

            Assert.IsFalse(same);
        }

        [Test]
        public void ArrayAndListWithSameValues()
        {
            int[] from = new int[] {1, 2, 3};
            List<int> to = new List<int> {1, 2, 3};

            var same = Mapping.Instance.MapEqual(from, to);

            Assert.IsTrue(same);
        }

		[Test]
		public void ValidateMapping()
		{
			Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
