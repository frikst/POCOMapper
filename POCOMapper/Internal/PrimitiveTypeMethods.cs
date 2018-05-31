using System;
using System.Reflection;

namespace KST.POCOMapper.@internal
{
	internal static class PrimitiveTypeMethods
	{
		public static MethodInfo Parse(Type primitiveType)
		{
			return primitiveType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null);
		}
	}
}
