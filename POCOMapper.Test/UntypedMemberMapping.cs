using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Object;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class UntypedMemberMapping
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
					.Member("s", "helloWorld")
					.Member("b", "number");
			}
		}

		[Test]
		public void UntypedMemberMappingTest()
		{
			From from = new From();

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual("s", to.helloWorld);
			Assert.AreEqual("a", to.a);
			Assert.AreEqual("1", to.number);
		}

		[Test]
		public void UntypedMemberMappingSynchronizationTest()
		{
			From from = new From();
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.AreEqual("s", to.helloWorld);
			Assert.AreEqual("a", to.a);
			Assert.AreEqual("1", to.number);
		}
	}
}
