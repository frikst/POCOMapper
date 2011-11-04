using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace POCOMapper.commonMappings
{
	public class EnumerableToList<TFrom, TTo> : IMapping<TFrom, TTo>
		where TFrom : class
		where TTo : class
	{
		private Func<TFrom, TTo> aFnc;
		private readonly MappingImplementation aMapping;

		public EnumerableToList(MappingImplementation mapping)
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
			Type itemTo = typeof(TTo).GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)).GetGenericArguments()[0];
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");
			ParameterExpression item = Expression.Parameter(typeof(TTo), "item");

			if (itemFrom != itemTo)
			{
				IMapping itemMapping = this.aMapping.GetMapping(itemFrom, itemTo);

				return Expression.Lambda<Func<TFrom, TTo>>(
					Expression.Call(null, typeof(Enumerable).GetMethod("ToList", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(itemTo),
						Expression.Call(null, typeof(Enumerable).GetMethod("Select", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(itemFrom, itemTo),
							from,
							Expression.Lambda(
								Expression.Call(
									Expression.Constant(itemMapping),
									typeof(IMapping<,>).MakeGenericType(itemFrom, itemTo).GetMethod("Map"),
									item
								),
								item
							)
						)
					),
					from
				).Compile();
			}
			else
			{
				return Expression.Lambda<Func<TFrom, TTo>>(
					Expression.Call(null, typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(itemTo),
						from
					),
					from
				).Compile();
			}
		}
	}
}
