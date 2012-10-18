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

		public override bool IsDirect
		{
			get { return false; }
		}

		protected override Expression<Func<TFrom, TTo>> CompileMapping()
		{
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");

			return this.CreateMappingEnvelope(
				from,
				Expression.Call(null, LinqMethods.ToArray(this.ItemTo),
					this.CreateItemMappingExpression(from)
				)
			);
		}
	}
}
