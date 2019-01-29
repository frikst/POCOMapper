using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Mapping.Standard;
using KST.POCOMapper.Validation;
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
		public void Mapping0ToBoolBoolTest()
		{
			From from = new From { i = 0 };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsFalse(to.i1);
			Assert.IsFalse(to.i2);
		}

		[Test]
		public void Mapping1ToBoolBoolTest()
		{
			From from = new From { i = 1 };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsTrue(to.i1);
			Assert.IsFalse(to.i2);
		}

		[Test]
		public void Mapping2ToBoolBoolTest()
		{
			From from = new From { i = 2 };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsFalse(to.i1);
			Assert.IsTrue(to.i2);
		}

		[Test]
		public void Mapping3ToBoolBoolTest()
		{
			From from = new From { i = 3 };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsTrue(to.i1);
			Assert.IsTrue(to.i2);
		}

		[Test]
		public void Synchronization0ToBoolBoolTest()
		{
			From from = new From { i = 0 };
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.IsFalse(to.i1);
			Assert.IsFalse(to.i2);
		}

		[Test]
		public void Synchronization1ToBoolBoolTest()
		{
			From from = new From { i = 1 };
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.IsTrue(to.i1);
			Assert.IsFalse(to.i2);
		}

		[Test]
		public void Synchronization2ToBoolBoolTest()
		{
			From from = new From { i = 2 };
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.IsFalse(to.i1);
			Assert.IsTrue(to.i2);
		}

		[Test]
		public void Synchronization3ToBoolBoolTest()
		{
			From from = new From { i = 3 };
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.IsTrue(to.i1);
			Assert.IsTrue(to.i2);
		}

		[Test]
		public void ValidateMapping()
		{
			Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
