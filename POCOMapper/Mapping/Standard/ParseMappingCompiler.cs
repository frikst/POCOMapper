using System;
using System.Linq.Expressions;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.MappingCompilaton;

namespace KST.POCOMapper.Mapping.Standard
{
	internal class ParseMappingCompiler<TTo> : MappingCompiler<string, TTo>
	{
		protected override Expression<Func<string, TTo>> CompileToExpression()
		{
			ParameterExpression from = Expression.Parameter(typeof(string), "from");

			if (typeof(TTo).IsEnum)
			{
				return Expression.Lambda<Func<string, TTo>>(
					Expression.Convert(
						Expression.Call(
							null,
							EnumMethods.Parse(typeof(TTo)),
							Expression.Constant(typeof(TTo)),
							from
						),
						typeof(TTo)
					),
					from
				);
			}
			else
			{
				return Expression.Lambda<Func<string, TTo>>(
					Expression.Call(
						null,
						PrimitiveTypeMethods.Parse(typeof(TTo)),
						from
					),
					from
				);
			}
		}
	}
}