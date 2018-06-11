﻿using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Special;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.POCOMapper.Test
{
	[TestClass]
	public class UntypedPostprocessTest
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
				Map(typeof(From), typeof(To))
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
			Mapping.Instance.Synchronize(new From(), ref ret);
			Assert.AreEqual("Hi", ret.Value);
		}
	}
}
