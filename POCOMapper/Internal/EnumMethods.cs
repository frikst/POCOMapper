using System;
using System.Reflection;

namespace KST.POCOMapper.@internal
{
	internal static class EnumMethods
	{
		private static readonly MethodInfo aParse = typeof(Enum).GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);

		public static MethodInfo Parse(Type enumType)
		{
			return aParse;
		}
	}
}
