using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
	public class NoAttributesMapping
	{
		private class From
		{

		}

		private class To
		{

		}

		private class Mapping : MappingDefinition
		{
			private Mapping()
			{
				Map<From, To>();
			}

			public static MappingImplementation Instance
			{
				get { return GetInstance<Mapping>(); }
			}
		}

		[TestMethod]
		public void CheckCorrectTypeTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
		}

		[TestMethod]
		public void ToStringTest()
		{
			string correct = "ObjectToObject`2<From, To>\n";

			string mappingToString = Mapping.Instance.MappingToString<From, To>();

			Assert.AreEqual(correct, mappingToString);
		}
	}
}
