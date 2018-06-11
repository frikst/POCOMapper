using System;
using System.Reflection;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Internal
{
	internal static class MappingMethods
	{
		public static MethodInfo Map(Type from, Type to)
		{
			return typeof(IMapping<,>).MakeGenericType(from, to).GetMethod("Map");
		}

		public static MethodInfo Synchronize(Type from, Type to)
		{
			return typeof(IMapping<,>).MakeGenericType(from, to).GetMethod("Synchronize");
		}
	}
}
