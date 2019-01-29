using KST.POCOMapper.Definition;
using KST.POCOMapper.Validation;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class TwoAttributesWithConflictingPrefix
	{
		private class FromChild
		{
			public int id;
		}

		private class From
		{
			public FromChild prefix;
		}

		private class To
		{
			public int prefixId;
			public int prefixChild;
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
			}
		}

		[Test]
		public void ConflictedMappingTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From { prefix = new FromChild { id = 5 } });
			Assert.AreEqual(5, ret.prefixId);
		}

		[Test]
		public void ValidateMapping()
		{
			Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
