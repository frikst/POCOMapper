using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace POCOMapper.commonMappings
{
	public class EnumerableToArray<TFrom, TTo> : IMapping<TFrom, TTo> where TFrom : class where TTo : class
	{
		private Func<TFrom, TTo> aFnc;
		private MappingImplementation aMapping;

		public EnumerableToArray(MappingImplementation mapping)
		{
			this.aFnc = null;
			this.aMapping = mapping;
		}

		#region Implementation of IMapping<in TFrom,out TTo>

		public TTo Map(TFrom from)
		{
			if (from == null)
				return null;

			if (this.aFnc == null)
				this.aFnc = this.Compile();

			return this.aFnc(from);
		}

		#endregion

		private Func<TFrom, TTo> Compile()
		{
			Type itemFrom = typeof(TFrom).GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)).GetGenericArguments()[0];
			Type itemTo = typeof(TTo).GetElementType();
			ParameterExpression from = Expression.Parameter(itemFrom, "from");
			ParameterExpression item = Expression.Parameter(itemFrom, "item");

			IMapping itemMapping = this.aMapping.GetMapping(itemFrom, itemTo);

			if (itemFrom != itemTo)
			{
				return Expression.Lambda<Func<TFrom, TTo>>(
					Expression.Call(null, typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(itemTo),
						Expression.Call(null, typeof(Enumerable).GetMethod("Select").MakeGenericMethod(itemFrom, itemTo),
							from,
							Expression.Lambda(
								Expression.Call(
									item,
									typeof(IMapping<,>).MakeGenericType(itemFrom, itemTo).GetMethod("Map")
								),
								item
							)
						)
					)
				).Compile();
			}
			else
			{
				return Expression.Lambda<Func<TFrom, TTo>>(
					Expression.Call(null, typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(itemTo),
						from
					)
				).Compile();
			}
		}
	}
}
