using KST.POCOMapper.Definition;
using KST.POCOMapper.Validation;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class SimpleComparision
	{
		private class From
		{
			private string aValue;

			public From(string value)
			{
				this.aValue = value;
			}

			public string GetValue()
			{
				return this.aValue;
			}
		}

		private class To
		{
			public string Value { get; set; }
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
			}
		}

		[Test]
		public void CompareSame()
		{
			From val = new From("Test");
			To valNew = new To { Value = "Test"};

			var same = Mapping.Instance.MapEqual(val, valNew);

			Assert.IsTrue(same);
		}

		[Test]
		public void CompareDifferent()
		{
			From val = new From("Test");
			To valNew = new To { Value = "Other"};

			var same = Mapping.Instance.MapEqual(val, valNew);

			Assert.IsFalse(same);
		}

		[Test]
		public void ValidateMapping()
		{
			Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
