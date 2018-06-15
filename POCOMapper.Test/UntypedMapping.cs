using KST.POCOMapper.Definition;
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

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map(typeof(From), typeof(To));
			}
		}

		[Test]
		public void CheckCorrectTypeTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
		}

		[Test]
		public void ToStringTest()
		{
			string correct = "ObjectToObject<From, To>";

			ToStringVisitor visitor = new ToStringVisitor();

			Mapping.Instance.AcceptForAll(visitor);

			string mappingToString = visitor.GetResult();

			Assert.AreEqual(correct, mappingToString);
		}
	}
}
