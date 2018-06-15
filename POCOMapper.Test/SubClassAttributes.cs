using KST.POCOMapper.Definition;
using KST.POCOMapper.Visitor;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
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

		[Test]
		public void MappingTest()
		{
			SubTo to = Mapping.Instance.Map<SubFrom, SubTo>(new SubFrom());
			Assert.AreEqual("from", to.Value);
			Assert.AreEqual("from2", to.Value2);
			Assert.AreEqual("from3", to.GetValue3());
		}

		[Test]
		public void ToStringTest()
		{
			string correct = "ObjectToObject<SubFrom, SubTo>\n    GetValue2 => Value2 (null)\n    GetValue => Value (null)\n    GetValue3 => value3 (null)";

			ToStringVisitor visitor = new ToStringVisitor();

			Mapping.Instance.AcceptForAll(visitor);

			string mappingToString = visitor.GetResult();

			Assert.AreEqual(correct, mappingToString);
		}
	}
}
