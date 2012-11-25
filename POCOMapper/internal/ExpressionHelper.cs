﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace POCOMapper.@internal
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
					Expression.Call(source, typeof(IEnumerable).GetMethod("GetEnumerator"))
				),

				Expression.Loop(
					Expression.IfThenElse(
						Expression.Call(enumerator, typeof(IEnumerator).GetMethod("MoveNext")),

						Expression.Block(
							Expression.Assign(item, Expression.Convert(Expression.Property(enumerator, "Current"), item.Type)),
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
	}
}
