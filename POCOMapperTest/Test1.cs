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

		public int[] aVal3 = new int[] { 1, 2, 3 };
	}
}
