using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Mapping.Standard;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class MemberMapping1toN
	{
		private class From
		{
			public int i;
		}

		private class To
		{
			public bool i1;
			public bool i2;
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.ObjectMappingRules()
					.MemberTo<bool>("i1", def => def
						.FuncMappingRules()
						.Using(from => (from.i & 1) == 1)
					)
					.MemberTo<bool>("i2", def => def
						.FuncMappingRules()
						.Using(from => (from.i & 2) == 2)
					);
			}
		}

		[Test]
		public void Mappping0Test()
		{
			From from = new From { i = 0 };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsFalse(to.i1);
			Assert.IsFalse(to.i2);
		}

		[Test]
		public void Mappping1Test()
		{
			From from = new From { i = 1 };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsTrue(to.i1);
			Assert.IsFalse(to.i2);
		}

		[Test]
		public void Mappping2Test()
		{
			From from = new From { i = 2 };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsFalse(to.i1);
			Assert.IsTrue(to.i2);
		}

		[Test]
		public void Mappping3Test()
		{
			From from = new From { i = 3 };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsTrue(to.i1);
			Assert.IsTrue(to.i2);
		}

		[Test]
		public void Synchronization0Test()
		{
			From from = new From { i = 0 };
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.IsFalse(to.i1);
			Assert.IsFalse(to.i2);
		}

		[Test]
		public void Synchronization1Test()
		{
			From from = new From { i = 1 };
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.IsTrue(to.i1);
			Assert.IsFalse(to.i2);
		}

		[Test]
		public void Synchronization2Test()
		{
			From from = new From { i = 2 };
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.IsFalse(to.i1);
			Assert.IsTrue(to.i2);
		}

		[Test]
		public void Synchronization3Test()
		{
			From from = new From { i = 3 };
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.IsTrue(to.i1);
			Assert.IsTrue(to.i2);
		}
	}
}
