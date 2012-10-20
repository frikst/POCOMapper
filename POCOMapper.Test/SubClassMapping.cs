using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;
using POCOMapper.exceptions;

namespace POCOMapper.Test
{
	[TestClass]
	public class SubClassMapping
	{
		private class From { }
		private class SubFrom1 : From { }
		private class SubFrom2 : From { }
		private class SubFrom3 : From { }
		private class SubFrom4 : From { }

		private class To { }
		private class SubTo1 : To { }
		private class SubTo2 : To { }

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.MapSubClass<SubFrom1, SubTo1>()
					.MapSubClass<SubFrom2, SubTo1>()
					.MapSubClass<SubFrom3, SubTo2>();

				Map<SubFrom1, SubTo1>();
				Map<SubFrom2, SubTo1>();
				Map<SubFrom3, SubTo2>();
			}
		}

		[TestMethod]
		public void ParentToParentTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
		}

		[TestMethod]
		public void SubFrom1ToSubTo1Test()
		{
			To ret = Mapping.Instance.Map<From, To>(new SubFrom1());
			Assert.AreEqual(typeof(SubTo1), ret.GetType());
		}

		[TestMethod]
		public void SubFrom2ToSubTo1Test()
		{
			To ret = Mapping.Instance.Map<From, To>(new SubFrom2());
			Assert.AreEqual(typeof(SubTo1), ret.GetType());
		}

		[TestMethod]
		public void SubFrom3ToSubTo2Test()
		{
			To ret = Mapping.Instance.Map<From, To>(new SubFrom3());
			Assert.AreEqual(typeof(SubTo2), ret.GetType());
		}

		[TestMethod]
		public void SubFrom4FailTest()
		{
			bool error = false;
			try
			{
				To ret = Mapping.Instance.Map<From, To>(new SubFrom4());
			}
			catch (UnknownMapping e)
			{
				error = true;
				Assert.AreEqual(typeof(SubFrom4), e.From);
				Assert.AreEqual(typeof(To), e.To);
			}
			Assert.IsTrue(error, "Should throw the UnknownMapping exception");
		}

		[TestMethod]
		public void ToStringTest()
		{
			string correct = "SubClassToObject`2<From, To>\n    SubFrom1 => SubTo1 ObjectToObject`2<SubFrom1, SubTo1>\n    SubFrom2 => SubTo1 ObjectToObject`2<SubFrom2, SubTo1>\n    SubFrom3 => SubTo2 ObjectToObject`2<SubFrom3, SubTo2>\n    From => To ObjectToObject`2<From, To>\n" + Constants.STANDARD_MAPPINGS;

			string mappingToString = Mapping.Instance.AllMappingsToString();

			Assert.AreEqual(correct, mappingToString);
		}
	}
}
