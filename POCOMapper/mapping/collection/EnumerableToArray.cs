using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using POCOMapper.definition;
using POCOMapper.@internal;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.collection
{
	public class EnumerableToArray<TFrom, TTo> : CompiledMapping<TFrom, TTo>
		where TFrom : class
		where TTo : class
	{
		public EnumerableToArray(MappingImplementation mapping)
			: base(mapping)
		{

		}

		protected override Expression<Func<TFrom, TTo>> Compile()
		{
			Type itemFrom = typeof(TFrom).GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)).GetGenericArguments()[0];
			Type itemTo = typeof(TTo).GetElementType();
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");
			ParameterExpression item = Expression.Parameter(typeof(TTo), "item");

			if (itemFrom != itemTo)
			{
				IMapping itemMapping = this.Mapping.GetMapping(itemFrom, itemTo);

				Delegate mapMethod = Delegate.CreateDelegate(
					typeof(Func<,>).MakeGenericType(itemFrom, itemTo),
					itemMapping,
					typeof(IMapping<,>).MakeGenericType(itemFrom, itemTo).GetMethod("Map")
				);

				return Expression.Lambda<Func<TFrom, TTo>>(
					Expression.Call(null, LinqMethods.ToArray.MakeGenericMethod(itemTo),
						Expression.Call(null, LinqMethods.Select.MakeGenericMethod(itemFrom, itemTo),
							from,
							Expression.Constant(mapMethod)
						)
					),
					from
				);
			}
			else
			{
				return Expression.Lambda<Func<TFrom, TTo>>(
					Expression.Call(null, typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(itemTo),
						from
					),
					from
				);
			}
		}
	}
}
