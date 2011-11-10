using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;

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
			Assert.AreEqual(ret.GetType(), typeof(SubTo1));
		}

		[TestMethod]
		public void SubFrom2ToSubTo1Test()
		{
			To ret = Mapping.Instance.Map<From, To>(new SubFrom2());
			Assert.AreEqual(ret.GetType(), typeof(SubTo1));
		}

		[TestMethod]
		public void SubFrom3ToSubTo2Test()
		{
			To ret = Mapping.Instance.Map<From, To>(new SubFrom3());
			Assert.AreEqual(ret.GetType(), typeof(SubTo2));
		}

		[TestMethod]
		public void SubFrom4FailTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new SubFrom4());
			Assert.IsNull(ret);
		}
	}
}
