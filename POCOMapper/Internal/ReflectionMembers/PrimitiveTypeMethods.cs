using System;
using System.Reflection;

namespace KST.POCOMapper.Internal.ReflectionMembers
{
	internal static class PrimitiveTypeMethods
	{
		public static MethodInfo Parse(Type primitiveType)
		{
			return primitiveType.GetMethod(nameof(int.Parse), BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null);
		}
	}
}
