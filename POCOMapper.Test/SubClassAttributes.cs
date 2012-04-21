using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
	public class SubClassAttributes
	{
		private class From
		{
			public string GetValue()
			{
				return "from";
			}

			public string GetValue3()
			{
				return "from3";
			}
		}
		private class SubFrom : From
		{
			public string GetValue2()
			{
				return "from2";
			}
		}

		private class To
		{
			public string Value2 { get; set; }

			private string value3;

			public string GetValue3()
			{
				return this.value3;
			}
		}

		private class SubTo : To
		{
			public string Value { get; set; }
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<SubFrom, SubTo>();
			}
		}

		[TestMethod]
		public void MappingTest()
		{
			SubTo to = Mapping.Instance.Map<SubFrom, SubTo>(new SubFrom());
			Assert.AreEqual("from", to.Value);
			Assert.AreEqual("from2", to.Value2);
			Assert.AreEqual("from3", to.GetValue3());
		}

		[TestMethod]
		public void ToStringTest()
		{
			string correct = "ObjectToObject`2<SubFrom, SubTo>\n    GetValue2 => Value2 (null)\n    GetValue => Value (null)\n    GetValue3 => value3 (null)\n";

			string mappingToString = Mapping.Instance.MappingToString<SubFrom, SubTo>();

			Assert.AreEqual(correct, mappingToString);
		}
	}
}
