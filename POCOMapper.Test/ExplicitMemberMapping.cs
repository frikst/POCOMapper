using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Validation;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class ExplicitMemberMapping
	{
		private class From
		{
			public string s = "s";
			public string a = "a";
			public int b = 1;
		}

		private class To
		{
			public string helloWorld;
			public string a;
			public string number;
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.ObjectMappingRules()
					.Member<string, string>("s", "helloWorld", def => { })
					.Member<int, string>("b", "number", def => { });
			}
		}

		[Test]
		public void ExplicitMemberMappingTest()
		{
			From from = new From();

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual("s", to.helloWorld);
			Assert.AreEqual("a", to.a);
			Assert.AreEqual("1", to.number);
		}

		[Test]
		public void ExplicitMemberSynchronizationTest()
		{
			From from = new From();
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.AreEqual("s", to.helloWorld);
			Assert.AreEqual("a", to.a);
			Assert.AreEqual("1", to.number);
		}

		[Test]
		public void ValidateMapping()
		{
			Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
