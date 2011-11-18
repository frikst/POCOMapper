using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
	public class SimpleSynchronization
	{
		private class From
		{
			public string GetValue()
			{
				return "from";
			}
		}

		private class To
		{
			public string Value { get; set; }
		}

		private class Mapping : MappingDefinition
		{
			private Mapping()
			{
				CreateMap<From, To>();
			}

			public static MappingImplementation Instance
			{
				get { return GetInstance<Mapping>(); }
			}
		}

		[TestMethod]
		public void TestMethod1()
		{
			From val = new From();
			To valNew = new To();

			Mapping.Instance.Synchronize(val, valNew);

			Assert.AreEqual(val.GetValue(), valNew.Value);
		}
	}
}
