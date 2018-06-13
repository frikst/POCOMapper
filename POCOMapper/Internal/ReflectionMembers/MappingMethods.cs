using System;
using System.Reflection;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Internal.ReflectionMembers
{
	internal static class MappingMethods
	{
		public static MethodInfo Map(Type from, Type to)
		{
			return typeof(IMapping<,>).MakeGenericType(from, to).GetMethod(nameof(IMapping<object, object>.Map));
		}

		public static MethodInfo Synchronize(Type from, Type to)
		{
			return typeof(IMapping<,>).MakeGenericType(from, to).GetMethod(nameof(IMapping<object, object>.Synchronize));
		}
	}
}
