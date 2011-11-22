using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace POCOMapper.@internal
{
	internal static class PrimitiveTypeMethods
	{
		public static MethodInfo Parse(Type primitiveType)
		{
			return primitiveType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null);
		}
	}
}
