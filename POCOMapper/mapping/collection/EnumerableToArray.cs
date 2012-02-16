using System;
using System.Linq.Expressions;
using POCOMapper.definition;
using POCOMapper.@internal;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.collection
{
	public class EnumerableToArray<TFrom, TTo> : CompiledCollectionMapping<TFrom, TTo>
		where TFrom : class
		where TTo : class
	{
		public EnumerableToArray(MappingImplementation mapping)
			: base(mapping)
		{

		}

		protected override Expression<Func<TFrom, TTo>> CompileMapping()
		{
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");

			IMapping itemMapping = this.GetMapping();

			if (itemMapping != null)
			{
				Delegate mapMethod = Delegate.CreateDelegate(
					typeof(Func<,>).MakeGenericType(this.ItemFrom, this.ItemTo),
					itemMapping,
					MappingMethods.Map(this.ItemFrom, this.ItemTo)
				);

				return Expression.Lambda<Func<TFrom, TTo>>(
					Expression.Call(null, LinqMethods.ToArray(this.ItemTo),
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
					Expression.Call(null, LinqMethods.ToArray(this.ItemTo),
						from
					),
					from
				);
			}
		}
	}
}
