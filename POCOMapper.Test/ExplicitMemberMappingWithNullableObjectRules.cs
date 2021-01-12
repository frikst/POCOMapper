using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Decorators;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Mapping.Standard;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	class ExplicitMemberMappingWithNullableObjectRules
	{
		private class FromInner
		{
			public string s;
		}

		private class From
		{
			public FromInner inner;
		}

		private class ToInner
		{
			public string s;
		}

		private class To
		{
			public ToInner inner;
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.ObjectMappingRules()
					.Member<FromInner, ToInner>("inner", "inner", def => def
						.NullableRules()
					);
			}
		}

		[Test]
		public void MapFromObject()
		{
			From from = new From {inner = new FromInner {s = "foo"}};

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsNotNull(to.inner);
			Assert.AreEqual("foo", to.inner.s);
		}

		[Test]
		public void MapFromNull()
		{
			From from = new From {inner = null};

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsNull(to.inner);
		}
	}
}
