﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
	public class Structuring
	{
		private class From
		{
			public string InnerData = "hello";
			public string Data = "world";
		}

		private class ToInner
		{
			public string Data;
		}

		private class To
		{
			public ToInner Inner;
			public string Data;
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
			}
		}

		[TestMethod]
		public void StructuringMapTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
			Assert.AreEqual("hello", ret.Inner.Data);
			Assert.AreEqual("world", ret.Data);
		}

		[TestMethod]
		public void StructuringSynchronizationTest()
		{
			To to = new To();
			From from = new From();

			Mapping.Instance.Synchronize(from, to);

			Assert.AreEqual("hello", to.Inner.Data);
			Assert.AreEqual("world", to.Data);
		}

		[TestMethod]
		public void StructuringToStringTest()
		{
			string correct = Constants.STANDARD_MAPPINGS + "ObjectToObject`2<From, To>\n    InnerData => Inner.Data (null)\n    Data => Data (null)\n" + Constants.SEPARATOR;

			string mappingToString = Mapping.Instance.AllMappingsToString();

			Assert.AreEqual(correct, mappingToString);
		}
	}
}