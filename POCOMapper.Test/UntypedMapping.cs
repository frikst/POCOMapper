using KST.POCOMapper.Definition;
using KST.POCOMapper.Validation;
using KST.POCOMapper.Visitor;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class UntypedMapping
	{
		private class From
		{

		}

		private class To
		{

		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map(typeof(From), typeof(To));
			}
		}

		[Test]
		public void NoAttributeUntypedMappingTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
		}

		[Test]
		public void UntypedMappingToStringTest()
		{
			string correct = "ObjectToObject<From, To>";

			ToStringVisitor visitor = new ToStringVisitor();

			Mapping.Instance.Mappings.AcceptForAll(visitor);

			string mappingToString = visitor.GetResult();

			Assert.AreEqual(correct, mappingToString);
		}

		[Test]
		public void ValidateMapping()
		{
			Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
