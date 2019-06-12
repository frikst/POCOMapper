using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Decorators;
using KST.POCOMapper.Validation;
using KST.POCOMapper.Visitor;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class RecursiveStructureEquality
	{
		private class From
		{
			public From Inner;
		}

		private class To
		{
			public To Inner;
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.NullableRules();
			}
		}

		[Test]
		public void MapEqualEmpty()
		{
			var from = new From { };
			var to = new To { };

			var same = Mapping.Instance.MapEqual(from, to);

			Assert.IsTrue(same);
		}

		[Test]
		public void MapEqualOneLevel()
		{
			var from = new From { Inner = new From()};
			var to = new To { Inner = new To()};

			var same = Mapping.Instance.MapEqual(from, to);

			Assert.IsTrue(same);
		}

		[Test]
		public void MapEqualDifferentLevels()
		{
			var from = new From { Inner = new From()};
			var to = new To { };

			var same = Mapping.Instance.MapEqual(from, to);

			Assert.IsFalse(same);
		}

		[Test]
		public void ValidateMapping()
		{
			Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
