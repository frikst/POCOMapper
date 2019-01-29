using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Decorators;
using KST.POCOMapper.Validation;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class CompositeStructure
	{
		private class From
		{
			public From child;
		}

		private class To
		{
			public To child;
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.NullableRules();
			}
		}

		[Test]
		public void CompositeMapping()
		{
			From from = new From { child = new From { child = new From() } };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsNotNull(to.child);
			Assert.IsNotNull(to.child.child);
			Assert.IsNull(to.child.child.child);
		}

		[Test]
		public void CompositeSynchronization()
		{
			From from = new From { child = new From { child = new From() } };
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.IsNotNull(to.child);
			Assert.IsNotNull(to.child.child);
			Assert.IsNull(to.child.child.child);
		}

		[Test]
		public void CompositeSynchronizationWithNonNullDestinationField()
		{
			From from = new From { child = new From { child = new From() } };
			To to = new To { child = new To() };
			To toChild = to.child;

			Mapping.Instance.Synchronize(from, ref to);
			Assert.IsNotNull(to.child);
			Assert.IsNotNull(to.child.child);
			Assert.IsNull(to.child.child.child);
			Assert.AreSame(toChild, to.child);
		}

		[Test]
		public void ValidateMapping()
		{
			Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
