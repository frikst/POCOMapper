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

			if (this.aToIEnumerable)
			{
				return this.CreateMappingEnvelope(
					from,
					this.CreateItemMappingExpression(from)
				);
			}
			else
			{
				ConstructorInfo constructTo = typeof(TTo).GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(this.ItemTo) });

				return this.CreateMappingEnvelope(
					from,
					Expression.New(constructTo,
						this.CreateItemMappingExpression(from)
					)
				);
			}
		}
	}
}
