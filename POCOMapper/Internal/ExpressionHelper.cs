using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace KST.POCOMapper.Internal
{
	internal class ExpressionHelper
	{
		public static Expression ForEach(ParameterExpression item, Expression source, Expression body)
		{
			ParameterExpression enumerator = Expression.Parameter(typeof(IEnumerator), "enumerator");
			LabelTarget forEachEnd = Expression.Label();

			return Expression.Block(
				new ParameterExpression[] { item, enumerator },

				Expression.Assign(
					enumerator,
					Expression.Call(source, typeof(IEnumerable).GetMethod(nameof(IEnumerable.GetEnumerator)))
				),

				Expression.Loop(
					Expression.IfThenElse(
						Expression.Call(enumerator, typeof(IEnumerator).GetMethod(nameof(IEnumerator.MoveNext))),

						Expression.Block(
							Expression.Assign(item, Expression.Convert(Expression.Property(enumerator, nameof(IEnumerator.Current)), item.Type)),
							body
						),

						Expression.Break(forEachEnd)
					),
					forEachEnd
				)
			);
		}

		public static MethodCallExpression Call(Delegate @delegate, params Expression[] arguments)
		{
			if (@delegate.Target == null)
				return Expression.Call(@delegate.Method, arguments);
			else
				return Expression.Call(Expression.Constant(@delegate.Target), @delegate.Method, arguments);
		}

		public static string GetDebugView(Expression expression)
		{
			PropertyInfo DebugViewProperty = typeof(Expression).GetProperty("DebugView", BindingFlags.NonPublic | BindingFlags.Instance);
			return (string) DebugViewProperty.GetValue(expression, null);
		}

		public static Expression Same(Expression left, Expression right)
		{
			if (left.Type != right.Type)
				throw new ArgumentException("left and right expressions should have the same types");

			if (left.Type.IsValueType)
				return Expression.Equal(left, right);
			else
				return Expression.ReferenceEqual(left, right);
		}

		public static Expression NotSame(Expression left, Expression right)
		{
			if (left.Type != right.Type)
				throw new ArgumentException("left and right expressions should have the same types");

			if (left.Type.IsValueType)
				return Expression.NotEqual(left, right);
			else
				return Expression.ReferenceNotEqual(left, right);
		}
	}
}
