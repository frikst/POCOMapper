using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
	public class StandardMappings
	{
		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				
			}
		}

		[TestMethod]
		public void DoubleToInt()
		{
			int ret = Mapping.Instance.Map<double, int>(1.5);
			Assert.AreEqual(1, ret);
		}

		[TestMethod]
		public void IntToDouble()
		{
			double ret = Mapping.Instance.Map<int, double>(1);
			Assert.AreEqual(1.0, ret);
		}

		[TestMethod]
		public void StringToInt()
		{
			int ret = Mapping.Instance.Map<string, int>("158");
			Assert.AreEqual(158, ret);
		}

		[TestMethod]
		public void IntToString()
		{
			string ret = Mapping.Instance.Map<int, string>(158);
			Assert.AreEqual("158", ret);
		}
	}
}
