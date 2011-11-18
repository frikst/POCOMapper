using System;
using System.Reflection;
using POCOMapper.mapping.@base;

namespace POCOMapper.@internal
{
	public static class MappingMethods
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
