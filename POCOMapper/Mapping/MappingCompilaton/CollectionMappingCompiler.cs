using System;
using System.Linq.Expressions;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.MappingCompilaton
{
	public abstract class CollectionMappingCompiler<TFrom, TTo> : MappingCompiler<TFrom, TTo>
	{
		private readonly IUnresolvedMapping aItemMapping;
		private readonly Delegate aChildPostprocessing;
		private readonly bool aMapNullToEmpty;

		protected CollectionMappingCompiler(IUnresolvedMapping itemMapping, Delegate childPostprocessing, bool mapNullToEmpty)
		{
			this.aItemMapping = itemMapping;
			this.aChildPostprocessing = childPostprocessing;
			this.aMapNullToEmpty = mapNullToEmpty;
		}

		protected override Expression<Func<TFrom, TTo>> CompileToExpression()
		{
			var from = Expression.Parameter(typeof(TFrom), "from");

			return Expression.Lambda<Func<TFrom, TTo>>(
				this.CreateNullHandlingExpression(
					from,
					this.CreateChildPostprocessingExpression(
						this.CreateCollectionInstantiationExpression(this.CreateItemMappingExpression(from))
					)
				),
				from
			);
		}

		protected abstract Expression CreateCollectionInstantiationExpression(Expression itemMappingExpression);
		protected abstract Expression CreateEmptyCollectionExpression();

		private Expression CreateItemMappingExpression(ParameterExpression from)
		{
			if (this.aItemMapping.ResolvedMapping is IDirectMapping)
				return from;

			var mapMethod = Delegate.CreateDelegate(
				typeof(Func<,>).MakeGenericType(EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType),
				this.aItemMapping.ResolvedMapping,
				MappingMethods.Map(EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType)
			);

			return Expression.Call(null, LinqMethods.Select(EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType),
				from,
				Expression.Constant(mapMethod)
			);
		}

		private Expression CreateChildPostprocessingExpression(Expression body)
		{
			if (this.aChildPostprocessing == null)
				return body;

			var ret = Expression.Parameter(typeof(TTo), "ret");
			var item = Expression.Parameter(EnumerableReflection<TTo>.ItemType, "item");

			return Expression.Block(
				new ParameterExpression[] {ret},

				Expression.Assign(ret, body),
				ExpressionHelper.ForEach(
					item,
					ret,
					ExpressionHelper.Call(this.aChildPostprocessing, ret, item)
				),
				ret
			);
		}

		private Expression CreateNullHandlingExpression(ParameterExpression from, Expression body)
		{
			if (!this.aMapNullToEmpty)
				return body;

			return Expression.Condition(
				Expression.Equal(from, Expression.Constant(null, from.Type)),
				this.CreateEmptyCollectionExpression(),
				body,
				typeof(TTo)
			);
		}
	}
}