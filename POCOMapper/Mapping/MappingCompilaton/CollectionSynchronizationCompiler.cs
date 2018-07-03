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

		protected CollectionSynchronizationCompiler(IUnresolvedMapping itemMapping, IEqualityRules equalityRules, Delegate childPostprocessing)
		{
			this.aItemMapping = itemMapping;
			(this.aSelectIdFrom, this.aSelectIdTo) = equalityRules.GetIdSelectors();
			this.aChildPostprocessing = childPostprocessing;
		}

		protected Expression CreateItemSynchronizationExpression(ParameterExpression from, ParameterExpression to)
		{
			if (this.aItemMapping.ResolvedMapping.IsDirect)
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

		protected Expression<Func<TFrom, TTo, TTo>> CreateSynchronizationEnvelope(ParameterExpression from, ParameterExpression to, Expression body)
		{
			ParameterExpression item = Expression.Parameter(EnumerableReflection<TTo>.ItemType, "item");
			if (this.aChildPostprocessing == null)
			{
				return Expression.Lambda<Func<TFrom, TTo, TTo>>(body, from, to);
			}
			else
			{
				return Expression.Lambda<Func<TFrom, TTo, TTo>>(
					Expression.Block(
						new[] { to },

						Expression.Assign(to, body),
						ExpressionHelper.ForEach(
							item,
							to,
							ExpressionHelper.Call(this.aChildPostprocessing, to, item)
						),
						to
					),
					from, to
				);
			}
		}
	}
}