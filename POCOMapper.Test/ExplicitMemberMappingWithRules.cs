using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Decorators;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Mapping.Standard;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	class ExplicitMemberMappingWithRules
	{
		private class From
		{
			public string s;
		}

		private class To
		{
			public string s;
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.ObjectMappingRules()
					.Member<string, string>("s", "s", def => def
						.NullableRules()
						.CopyRules()
					);
			}
		}

		[Test]
		public void MapFromString()
		{
			From from = new From {s = "foo"};

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual("foo", to.s);
		}

		[Test]
		public void MapFromNull()
		{
			From from = new From {s = null};

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsNull(to.s);
		}
	}
}
