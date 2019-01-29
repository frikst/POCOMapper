using KST.POCOMapper.Definition;
using KST.POCOMapper.Validation;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class SimpleSynchronization
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
		public void SyncToEmpty()
		{
			From val = new From("test");
			To valNew = new To();

			Mapping.Instance.Synchronize(val, ref valNew);

			Assert.AreEqual(val.GetValue(), valNew.Value);
		}

		[Test]
		public void SyncToNonEmpty()
		{
			From val = new From("test");
			To valNew = new To();
			valNew.Value = "hello";

			Mapping.Instance.Synchronize(val, ref valNew);

			Assert.AreEqual(val.GetValue(), valNew.Value);
		}

		[Test]
		public void SyncEmptyToEmpty()
		{
			From val = new From(null);
			To valNew = new To();

			Mapping.Instance.Synchronize(val, ref valNew);

			Assert.AreEqual(val.GetValue(), valNew.Value);
		}

		[Test]
		public void SyncEmptyToNonEmpty()
		{
			From val = new From(null);
			To valNew = new To();
			valNew.Value = "hello";

			Mapping.Instance.Synchronize(val, ref valNew);

			Assert.AreEqual(val.GetValue(), valNew.Value);
		}

		[Test]
		public void ValidateMapping()
		{
			Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
