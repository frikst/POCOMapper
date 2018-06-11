using System;
using System.Linq.Expressions;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Internal;

namespace KST.POCOMapper.Mapping.Collection
{
	public class EnumerableToList<TFrom, TTo> : CompiledCollectionMapping<TFrom, TTo>
	{
		public EnumerableToList(MappingImplementation mapping, Delegate selectIdFrom, Delegate selectIdTo)
			: base(mapping, selectIdFrom, selectIdTo)
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
				Expression.Call(null, LinqMethods.ToList(this.ItemTo),
					this.CreateItemMappingExpression(from)
				)
			);
		}
	}
}
