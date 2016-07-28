using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMapper.Test
{
	internal static class Constants
	{
		public static readonly string SEPARATOR = "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";

		public static readonly string STANDARD_MAPPINGS = SEPARATOR
															+ "Cast<Int32, Double>\n" + SEPARATOR
															+ "Cast<Double, Int32>\n" + SEPARATOR
															+ "ToString<Int32>\n" + SEPARATOR
															+ "Parse<Int32>\n" + SEPARATOR
															+ "Copy<String>";
	}
}
