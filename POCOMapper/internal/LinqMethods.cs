using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace POCOMapper.@internal
{
	internal static class LinqMethods
	{
		public static readonly MethodInfo ToList = GetLinqMethod(x => x.ToList());
		public static readonly MethodInfo ToArray = GetLinqMethod(x => x.ToArray());
		public static readonly MethodInfo Select = GetLinqMethod(x => x.Select(obj => obj));

		private static MethodInfo GetLinqMethod<T>(Expression<Func<IEnumerable<int>, T>> expression)
		{
			return ((MethodCallExpression)expression.Body).Method.GetGenericMethodDefinition();
		}
	}
}
