using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
	public class ExplicitMappingPrecedence
	{
		private class From
		{
			public string attr = "incorrect";
			public string attrSrc = "correct";
		}

		private class To
		{
			public string attr;
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.Member<string, string>("attrSrc", "attr", def => { });
			}
		}

		[TestMethod]
		public void MapppingTest()
		{
			From from = new From();

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual("correct", to.attr);
		}

		[TestMethod]
		public void ToStringTest()
		{
			string correct = "ObjectToObject`2<From, To>\n    attrSrc => attr (null)\n" + Constants.STANDARD_MAPPINGS;

			string mappingToString = Mapping.Instance.AllMappingsToString();

			Assert.AreEqual(correct, mappingToString);
		}
	}
}
