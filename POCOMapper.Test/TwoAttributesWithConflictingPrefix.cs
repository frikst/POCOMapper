using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
	public class TwoAttributesWithConflictingPrefix
	{
		private class FromChild
		{
			public int id;
		}

		private class From
		{
			public FromChild prefix;
		}

		private class To
		{
			public int prefixId;
			public int prefixChild;
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
			}
		}

		[TestMethod]
		public void MappingTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From { prefix = new FromChild { id = 5 } });
			Assert.AreEqual(5, ret.prefixId);
		}
	}
}
