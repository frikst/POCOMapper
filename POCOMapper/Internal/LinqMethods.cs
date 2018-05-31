using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace POCOMapper.@internal
{
	internal static class LinqMethods
	{
		private static readonly MethodInfo aToList = GetLinqMethod(x => x.ToList());
		private static readonly MethodInfo aToArray = GetLinqMethod(x => x.ToArray());
		private static readonly MethodInfo aSelect = GetLinqMethod(x => x.Select(obj => obj));

		private static MethodInfo GetLinqMethod<TItem>(Expression<Func<IEnumerable<int>, TItem>> expression)
		{
			return ((MethodCallExpression)expression.Body).Method.GetGenericMethodDefinition();
		}

		public static MethodInfo ToList(Type itemType)
		{
			return aToList.MakeGenericMethod(itemType);
		}

		public static MethodInfo ToArray(Type itemType)
		{
			return aToArray.MakeGenericMethod(itemType);
		}

		public static MethodInfo Select(Type itemFrom, Type itemTo)
		{
			return aSelect.MakeGenericMethod(itemFrom, itemTo);
		}
	}
}
