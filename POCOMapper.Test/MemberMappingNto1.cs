using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Mapping.Special;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class MemberMappingNto1
	{
		private class From
		{
			public bool i1;
			public bool i2;
		}

		private class To
		{
			public int i;
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.ObjectMappingRules()
					.MemberTo<int>("i", def => def
						.FuncMappingRules()
						.Using(from => (from.i1 ? 1 : 0) | (from.i2 ? 2 : 0))
					);
			}
		}

		[Test]
		public void Mappping0Test()
		{
			From from = new From { i1 = false, i2 = false };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual(0, to.i);
		}

		[Test]
		public void Mappping1Test()
		{
			From from = new From { i1 = true, i2 = false };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual(1, to.i);
		}

		[Test]
		public void Mappping2Test()
		{
			From from = new From { i1 = false, i2 = true };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual(2, to.i);
		}

		[Test]
		public void Mappping3Test()
		{
			From from = new From { i1 = true, i2 = true };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual(3, to.i);
		}

		[Test]
		public void Synchronization0Test()
		{
			From from = new From { i1 = false, i2 = false };
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.AreEqual(0, to.i);
		}

		[Test]
		public void Synchronization1Test()
		{
			From from = new From { i1 = true, i2 = false };
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.AreEqual(1, to.i);
		}

		[Test]
		public void Synchronization2Test()
		{
			From from = new From { i1 = false, i2 = true };
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.AreEqual(2, to.i);
		}

		[Test]
		public void Synchronization3Test()
		{
			From from = new From { i1 = true, i2 = true };
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.AreEqual(3, to.i);
		}
	}
}
