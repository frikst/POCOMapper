using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using POCOMapper.definition;
using POCOMapper.@internal;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.collection
{
	public class EnumerableToEnumerable<TFrom, TTo> : CompiledCollectionMapping<TFrom, TTo>
		where TFrom : class
		where TTo : class
	{
		private readonly bool aToIEnumerable;

		public EnumerableToEnumerable(MappingImplementation mapping)
			: base(mapping)
		{
			if (typeof(TTo).IsGenericType && typeof(TTo).GetGenericTypeDefinition() == typeof(IEnumerable<>))
				this.aToIEnumerable = true;
			else
				this.aToIEnumerable = false;
		}

		protected override Expression<Func<TFrom, TTo>> CompileMapping()
		{
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");

			ConstructorInfo constructTo;

			if (this.aToIEnumerable)
				constructTo = typeof(List<>).MakeGenericType(this.ItemTo).GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(this.ItemTo) });
			else
				constructTo = typeof(TTo).GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(this.ItemTo) });

			IMapping itemMapping = this.GetMapping();

			if (itemMapping != null)
			{
				Delegate mapMethod = Delegate.CreateDelegate(
					typeof(Func<,>).MakeGenericType(this.ItemFrom, this.ItemTo),
					itemMapping,
					MappingMethods.Map(this.ItemFrom, this.ItemTo)
				);

				return Expression.Lambda<Func<TFrom, TTo>>(
					Expression.New(constructTo,
						Expression.Call(null, LinqMethods.Select(this.ItemFrom, this.ItemTo),
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
					Expression.New(constructTo,
						from
					),
					from
				);
			}
		}
	}
}
