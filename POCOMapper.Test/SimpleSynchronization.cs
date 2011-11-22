using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
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

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
			}
		}

		[TestMethod]
		public void SyncToEmpty()
		{
			From val = new From("test");
			To valNew = new To();

			Mapping.Instance.Synchronize(val, valNew);

			Assert.AreEqual(val.GetValue(), valNew.Value);
		}

		[TestMethod]
		public void SyncToNonEmpty()
		{
			From val = new From("test");
			To valNew = new To();
			valNew.Value = "hello";

			Mapping.Instance.Synchronize(val, valNew);

			Assert.AreEqual(val.GetValue(), valNew.Value);
		}

		[TestMethod]
		public void SyncEmptyToEmpty()
		{
			From val = new From(null);
			To valNew = new To();

			Mapping.Instance.Synchronize(val, valNew);

			Assert.AreEqual(val.GetValue(), valNew.Value);
		}

		[TestMethod]
		public void SyncEmptyToNonEmpty()
		{
			From val = new From(null);
			To valNew = new To();
			valNew.Value = "hello";

			Mapping.Instance.Synchronize(val, valNew);

			Assert.AreEqual(val.GetValue(), valNew.Value);
		}
	}
}
