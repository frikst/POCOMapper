using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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

		private class Mapping : MappingDefinition
		{
			private Mapping()
			{
				CreateMap<From, To>()
					.MapSubClass<SubFrom1, SubTo1>()
					.MapSubClass<SubFrom2, SubTo1>()
					.MapSubClass<SubFrom3, SubTo2>();

				CreateMap<SubFrom1, SubTo1>();
				CreateMap<SubFrom2, SubTo1>();
				CreateMap<SubFrom3, SubTo2>();
			}

			public static MappingImplementation Instance
			{
				get { return GetInstance<Mapping>(); }
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
	}
}
