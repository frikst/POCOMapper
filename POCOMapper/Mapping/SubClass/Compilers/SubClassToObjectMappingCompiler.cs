using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.MappingCompilaton;

namespace KST.POCOMapper.Mapping.SubClass.Compilers
{
	internal class SubClassToObjectMappingCompiler<TFrom, TTo> :MappingCompiler<TFrom, TTo>
	{
		private readonly IEnumerable<SubClassConversion> aConversions;

		public SubClassToObjectMappingCompiler(IEnumerable<SubClassConversion> conversions)
		{
			this.aConversions = conversions;
		}

		protected override Expression<Func<TFrom, TTo>> CompileToExpression()
	    {
		    var allConversions = this.aConversions.ToList();

		    var from = Expression.Parameter(typeof(TFrom), "from");
		    var to = Expression.Parameter(typeof(TTo), "to");

		    LabelTarget mappingEnd = Expression.Label();

		    return Expression.Lambda<Func<TFrom, TTo>>(
			    Expression.Block(
				    new ParameterExpression[] { to },
				    Expression.Block(
					    allConversions.Select(x => this.MakeIfConvertMapStatement(x.From, x.To, x.Mapping.ResolvedMapping, from, to, mappingEnd))
				    ),
				    Expression.Throw(
					    Expression.New(
						    typeof(UnknownMappingException).GetConstructor(new Type[] { typeof(Type), typeof(Type) }),
						    Expression.Call(from, ObjectMethods.GetType()),
						    Expression.Constant(typeof(TTo))
					    )
				    ),
				    Expression.Label(mappingEnd),
				    to
			    ),
			    from
		    );
	    }

		private Expression MakeIfConvertMapStatement(Type fromType, Type toType, IMapping mapping, ParameterExpression from, ParameterExpression to, LabelTarget mappingEnd)
		{
			return Expression.IfThen(
				Expression.Equal(
					Expression.Call(from, ObjectMethods.GetType()),
					Expression.Constant(fromType)
				),
				Expression.Block(
					Expression.Assign(
						to,
						Expression.Call(
							Expression.Constant(mapping),
							MappingMethods.Map(fromType, toType),
							Expression.Convert(from, fromType)
						)
					),
					Expression.Goto(mappingEnd)
				)
			);
		}
    }
}
