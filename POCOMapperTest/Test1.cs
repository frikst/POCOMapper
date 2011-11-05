using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMapperTest
{
	public class Test1
	{
		public string Val1
		{
			get { return "hello"; }
		}

		public string GetVal2()
		{
			return "hello from 2";
		}

		public Test1Child[] aVal3 = new Test1Child[] { new Test1Child { aName = "Hi" }, null, new Test1Child { aName = "Hello" } };
	}

	public class Test1Child
	{
		public string aName;
	}
}
