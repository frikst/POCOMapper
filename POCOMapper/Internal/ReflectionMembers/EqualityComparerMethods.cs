using System;
using System.Collections.Generic;
using System.Reflection;

namespace KST.POCOMapper.Internal.ReflectionMembers
{
    internal static class EqualityComparerMethods
    {
        public static MethodInfo Equals(Type type)
        {
            return typeof(IEqualityComparer<>).MakeGenericType(type).GetMethod(nameof(IEqualityComparer<object>.Equals));
        }
    }
}
