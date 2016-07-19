using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
	public class UntypedMapping
	{
		private class From
		{

		}

		private class To
		{

		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map(typeof(From), typeof(To));
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
			string correct = "ObjectToObject`2<From, To>\n" + Constants.STANDARD_MAPPINGS;

			string mappingToString = Mapping.Instance.AllMappingsToString();

			Assert.AreEqual(correct, mappingToString);
		}
	}
}
