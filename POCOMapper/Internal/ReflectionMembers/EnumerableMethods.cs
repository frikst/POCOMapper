using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace KST.POCOMapper.Internal.ReflectionMembers
{
    internal static class EnumerableMethods
    {
        public static MethodInfo GetEnumerable(Type itemType)
        {
            return typeof(IEnumerable<>).MakeGenericType(itemType).GetMethod(nameof(IEnumerable<object>.GetEnumerator));
        }

        public static MethodInfo MoveNext(Type itemType)
        {
            return typeof(IEnumerator).GetMethod(nameof(IEnumerator.MoveNext));
        }

        public static PropertyInfo Current(Type itemType)
        {
            return typeof(IEnumerator<>).MakeGenericType(itemType).GetProperty(nameof(IEnumerator<object>.Current));
        }
    }
}
