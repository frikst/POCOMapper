using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMapper.Test
{
	internal static class Constants
	{
		public static readonly string SEPARATOR = "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";

		public static readonly string STANDARD_MAPPINGS = "Cast`2<Int32, Double>\n" + SEPARATOR
															+ "Cast`2<Double, Int32>\n" + SEPARATOR
															+ "ToString`1<Int32>\n" + SEPARATOR
															+ "Parse`1<Int32>\n" + SEPARATOR
															+ "Copy`1<String>\n" + SEPARATOR;
	}
}
