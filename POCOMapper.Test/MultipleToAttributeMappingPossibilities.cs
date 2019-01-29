using KST.POCOMapper.Definition;
using KST.POCOMapper.Validation;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class MultipleToAttributeMappingPossibilities
	{
		private class From
		{
			public string DataData = "hello world";
		}

		private class ToInner
		{
			public string Data;
		}

		private class ToSuper
		{
			public ToInner Data;
		}

		private class To : ToSuper
		{
			public string DataData;
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
			}
		}

		[Test]
		public void StructuringWithMultipleToPossibilitiesMappingTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
			Assert.AreEqual(null, ret.Data);
			Assert.AreEqual("hello world", ret.DataData);
		}

		[Test]
		public void ValidateMapping()
		{
			Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
