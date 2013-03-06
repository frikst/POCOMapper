using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;
using POCOMapper.mapping.special;

namespace POCOMapper.Test
{
	[TestClass]
	public class PostprocessTest
	{
		private class From
		{
			
		}

		private class To
		{
			public string Value { get; set; }
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.PostprocessRules()
					.Postprocess((a, b) => b.Value = "Hi");
			}
		}

		[TestMethod]
		public void TestPostprocessMap()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
			Assert.AreEqual("Hi", ret.Value);
		}

		[TestMethod]
		public void TestPostprocessSnychronize()
		{
			To ret = new To();
			Mapping.Instance.Synchronize(new From(), ret);
			Assert.AreEqual("Hi", ret.Value);
		}
	}
}
