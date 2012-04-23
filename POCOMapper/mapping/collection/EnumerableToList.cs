using System;
using System.Linq.Expressions;
using POCOMapper.definition;
using POCOMapper.@internal;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.collection
{
	public class EnumerableToList<TFrom, TTo> : CompiledCollectionMapping<TFrom, TTo>
	{
		public EnumerableToList(MappingImplementation mapping)
			: base(mapping)
		{

		}

		protected override Expression<Func<TFrom, TTo>> CompileMapping()
		{
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");

			return this.CreateMappingEnvelope(
				from,
				Expression.Call(null, LinqMethods.ToList(this.ItemTo),
					this.CreateItemMappingExpression(from)
				)
			);
		}
	}
}
