using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace POCOMapper.commonMappings
{
	public class ObjectToObject<TFrom, TTo> : CompiledMapping<TFrom, TTo>
	{
		public ObjectToObject(MappingImplementation mapping)
			: base(mapping)
		{

		}

		protected override Func<TFrom, TTo> Compile()
		{
			ConstructorInfo constructor = typeof(TTo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { }, null);

			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");
			ParameterExpression to = Expression.Parameter(typeof(TTo), "to");

			LabelTarget fncEnd = Expression.Label();

			return Expression.Lambda<Func<TFrom, TTo>>(
				Expression.Block(
					new ParameterExpression[] { to },
					new Expression[]
					{
						Expression.Assign(
							to,
							Expression.New(constructor)
						),
						to
					}
				),
				from
			).Compile();
		}
	}
}
