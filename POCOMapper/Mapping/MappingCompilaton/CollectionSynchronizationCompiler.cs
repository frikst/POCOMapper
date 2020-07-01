using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.SpecialRules;

namespace KST.POCOMapper.Mapping.MappingCompilaton
{
	public abstract class CollectionSynchronizationCompiler<TFrom, TTo> : SynchronizationCompiler<TFrom, TTo>
	{
		private readonly IUnresolvedMapping aItemMapping;
		private readonly Delegate aSelectIdFrom;
		private readonly Delegate aSelectIdTo;
		private readonly Delegate aChildPostprocessing;
		private readonly bool aMapNullToEmpty;

		protected CollectionSynchronizationCompiler(IUnresolvedMapping itemMapping, IEqualityRules equalityRules, Delegate childPostprocessing, bool mapNullToEmpty)
		{
			this.aItemMapping = itemMapping;
			(this.aSelectIdFrom, this.aSelectIdTo) = equalityRules.GetIdSelectors();
			this.aChildPostprocessing = childPostprocessing;
			this.aMapNullToEmpty = mapNullToEmpty;
		}
		protected override Expression<Func<TFrom, TTo, TTo>> CompileToExpression()
		{
			var from = Expression.Parameter(typeof(TFrom), "from");
			var to = Expression.Parameter(typeof(TTo), "to");

			return Expression.Lambda<Func<TFrom, TTo, TTo>>(
				this.CreateChildPostprocessingExpression(
					this.CreateCollectionInstantiationExpression(
						this.CreateItemSynchronizationExpression(from, to)
					)
				),
				from,
				to
			);
		}

		protected abstract Expression CreateCollectionInstantiationExpression(Expression itemSynchronizationExpression);
		protected abstract Expression CreateEmptyCollectionExpression();

		protected Expression CreateItemSynchronizationExpression(ParameterExpression from, ParameterExpression to)
		{
			if (this.aItemMapping.ResolvedMapping is IDirectMapping)
				return from;

			var idType = this.aSelectIdFrom.Method.ReturnType;
			var synEnu = typeof(SynchronizationEnumerable<,,>).MakeGenericType(idType, EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType);
			var synEnuConstructor = synEnu.GetConstructor(
				BindingFlags.Public | BindingFlags.Instance,
				null,
				new [] {
					typeof(IEnumerable<>).MakeGenericType(EnumerableReflection<TFrom>.ItemType),
					typeof(IEnumerable<>).MakeGenericType(EnumerableReflection<TTo>.ItemType),
					typeof(Func<,>).MakeGenericType(EnumerableReflection<TTo>.ItemType, idType),
					typeof(Func<,>).MakeGenericType(EnumerableReflection<TFrom>.ItemType, idType),
					typeof(IMapping<,>).MakeGenericType(EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType)
				},
				null
			);

			return Expression.New(
				synEnuConstructor,
				Expression.Convert(from, typeof(IEnumerable<>).MakeGenericType(EnumerableReflection<TFrom>.ItemType)),
				Expression.Convert(to, typeof(IEnumerable<>).MakeGenericType(EnumerableReflection<TTo>.ItemType)),
				Expression.Constant(this.aSelectIdTo),
				Expression.Constant(this.aSelectIdFrom),
				Expression.Constant(this.aItemMapping.ResolvedMapping)
			);
		}

		protected Expression CreateChildPostprocessingExpression(Expression body)
		{
			if (this.aChildPostprocessing == null)
				return body;

			var ret = Expression.Parameter(EnumerableReflection<TTo>.ItemType, "ret");
			var item = Expression.Parameter(EnumerableReflection<TTo>.ItemType, "item");

			return Expression.Block(
				new[] {ret},

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

			return Expression.IfThenElse(
				Expression.Equal(from, Expression.Constant(null, from.Type)),
				this.CreateEmptyCollectionExpression(),
				body
			);
		}
	}
}