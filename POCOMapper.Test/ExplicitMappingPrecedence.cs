﻿using KST.POCOMapper.definition;
using KST.POCOMapper.mapping.common;
using KST.POCOMapper.visitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.POCOMapper.Test
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
					.ObjectMappingRules()
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
			string correct = "ObjectToObject<From, To>\n    attrSrc => attr (null)\n" + Constants.STANDARD_MAPPINGS;

			ToStringVisitor visitor = new ToStringVisitor();

			Mapping.Instance.AcceptForAll(visitor);

			string mappingToString = visitor.GetResult();

			Assert.AreEqual(correct, mappingToString);
		}
	}
}
