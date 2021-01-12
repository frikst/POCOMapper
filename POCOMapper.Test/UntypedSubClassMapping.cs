using KST.POCOMapper.Definition;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Mapping.SubClass;
using KST.POCOMapper.Validation;
using KST.POCOMapper.Visitor;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class UntypedSubClassMapping
	{
		private class From { }
		private class SubFrom1 : From { }
		private class SubFrom2 : From { }
		private class SubFrom3 : From { }
		private class SubFrom4 : From { }

		private class To { }
		private class SubTo1 : To { }
		private class SubTo2 : To { }

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map(typeof(From), typeof(To))
					.SubClassMappingRules()
					.Map(typeof(SubFrom1), typeof(SubTo1))
					.Map(typeof(SubFrom2), typeof(SubTo1))
					.Map(typeof(SubFrom3), typeof(SubTo2));

				Map<SubFrom1, SubTo1>();
				Map<SubFrom2, SubTo1>();
				Map<SubFrom3, SubTo2>();
			}
		}

		[Test]
		public void ParentToParentMappingTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
		}

		[Test]
		public void SubFrom1ToSubTo1MappingTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new SubFrom1());
			Assert.AreEqual(typeof(SubTo1), ret.GetType());
		}

		[Test]
		public void SubFrom2ToSubTo1MappingTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new SubFrom2());
			Assert.AreEqual(typeof(SubTo1), ret.GetType());
		}

		[Test]
		public void SubFrom3ToSubTo2MappingTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new SubFrom3());
			Assert.AreEqual(typeof(SubTo2), ret.GetType());
		}

		[Test]
		public void SubFrom4FailMappingTest()
		{
			bool error = false;
			try
			{
				To ret = Mapping.Instance.Map<From, To>(new SubFrom4());
			}
			catch (UnknownMappingException e)
			{
				error = true;
				Assert.AreEqual(typeof(SubFrom4), e.From);
				Assert.AreEqual(typeof(To), e.To);
			}
			Assert.IsTrue(error, "Should throw the UnknownMapping exception");
		}

		[Test]
		public void SubClassMappingToStringTest()
		{
			string correct = "SubClassToObject<From, To>\n    SubFrom1 => SubTo1 ObjectToObject<SubFrom1, SubTo1>\n    SubFrom2 => SubTo1 ObjectToObject<SubFrom2, SubTo1>\n    SubFrom3 => SubTo2 ObjectToObject<SubFrom3, SubTo2>\n    From => To ObjectToObject<From, To>\n"
				+ Constants.SEPARATOR + "ObjectToObject<SubFrom1, SubTo1>\n"
				+ Constants.SEPARATOR + "ObjectToObject<SubFrom2, SubTo1>\n"
				+ Constants.SEPARATOR + "ObjectToObject<SubFrom3, SubTo2>";

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
