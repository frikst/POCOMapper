using System;
using System.Linq.Expressions;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.MappingCompilaton
{
	public abstract class CollectionMappingCompiler<TFrom, TTo> : MappingCompiler<TFrom, TTo>
	{
		private readonly IMapping aItemMapping;
		private readonly Delegate aChildPostprocessing;

		protected CollectionMappingCompiler(IMapping itemMapping, Delegate childPostprocessing)
		{
			this.aItemMapping = itemMapping;
			this.aChildPostprocessing = childPostprocessing;
		}

		protected Expression CreateItemMappingExpression(ParameterExpression from)
		{
			if (this.aItemMapping.IsDirect)
				return from;

			var mapMethod = Delegate.CreateDelegate(
				typeof(Func<,>).MakeGenericType(EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType),
				this.aItemMapping,
				MappingMethods.Map(EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType)
			);

			return Expression.Call(null, LinqMethods.Select(EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType),
				from,
				Expression.Constant(mapMethod)
			);
		}

		protected Expression<Func<TFrom, TTo>> CreateMappingEnvelope(ParameterExpression from, Expression body)
		{
			if (this.aChildPostprocessing == null)
			{
				return Expression.Lambda<Func<TFrom, TTo>>(body, from);
			}
			else
			{
				ParameterExpression to = Expression.Parameter(typeof(TTo), "to");
				ParameterExpression item = Expression.Parameter(EnumerableReflection<TTo>.ItemType, "item");

				return Expression.Lambda<Func<TFrom, TTo>>(
					Expression.Block(
						new ParameterExpression[] { to },

						Expression.Assign(to, body),
						ExpressionHelper.ForEach(
							item,
							to,
							ExpressionHelper.Call(this.aChildPostprocessing, to, item)
						),
						to
					),
					from
				);
			}
		}
	}
}