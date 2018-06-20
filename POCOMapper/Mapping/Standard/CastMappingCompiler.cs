using System;
using System.Linq.Expressions;
using KST.POCOMapper.Mapping.MappingCompilaton;

namespace KST.POCOMapper.Mapping.Standard
{
	internal class CastMappingCompiler<TFrom, TTo> : MappingCompiler<TFrom, TTo>
	{
		protected override Expression<Func<TFrom, TTo>> CompileToExpression()
		{
			var from = Expression.Parameter(typeof(TFrom), "from");

			return Expression.Lambda<Func<TFrom, TTo>>(
				Expression.Convert(from, typeof(TTo)),
				from
			);
		}
	}
}