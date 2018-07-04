using System;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Object;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class FactoryMethod
	{
		private class From
		{

		}

		private class To
		{
			public int a;

			public To(int a)
			{
				this.a = a;
			}
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.ObjectMappingRules()
					.Factory(from => new To(5));
			}
		}

		private class MappingUntyped : MappingSingleton<MappingUntyped>
		{
			private MappingUntyped()
			{
				Map(typeof(From), typeof(To))
					.ObjectMappingRules()
					.Factory((from, toType) => Activator.CreateInstance(toType, 5));
			}
		}

		[Test]
		public void MapppingTest()
		{
			From from = new From();

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual(5, to.a);
		}

		[Test]
		public void UntypedMapppingTest()
		{
			From from = new From();

			To to = MappingUntyped.Instance.Map<From, To>(from);
			Assert.AreEqual(5, to.a);
		}
	}
}
