using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.MappingCompilaton;

namespace KST.POCOMapper.Mapping.SubClass.Compilers
{
    internal class SubClassToObjectSynchronizationCompiler<TFrom, TTo> : SynchronizationCompiler<TFrom, TTo>
    {
	    private readonly IEnumerable<SubClassConversion> aConversions;

	    public SubClassToObjectSynchronizationCompiler(IEnumerable<SubClassConversion> conversions)
	    {
		    this.aConversions = conversions;
	    }

	    protected override Expression<Func<TFrom, TTo, TTo>> CompileToExpression()
	    {
		    var allConversions = this.aConversions.ToList();

		    var from = Expression.Parameter(typeof(TFrom), "from");
		    var to = Expression.Parameter(typeof(TTo), "to");

		    var mappingEnd = Expression.Label();

		    return Expression.Lambda<Func<TFrom, TTo, TTo>>(
			    Expression.Block(
				    Expression.Block(
					    allConversions.Select(x => this.MakeIfConvertSynchronizeStatement(x.From, x.To, x.Mapping, from, to, mappingEnd))
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
			    from, to
		    );
	    }

	    private Expression MakeIfConvertSynchronizeStatement(Type fromType, Type toType, IMapping mapping, ParameterExpression from, ParameterExpression to, LabelTarget mappingEnd)
	    {
		    return Expression.IfThen(
			    Expression.Equal(
				    Expression.Call(from, ObjectMethods.GetType()),
				    Expression.Constant(fromType)
			    ),
			    Expression.Block(
				    Expression.Call(
					    Expression.Constant(mapping),
					    MappingMethods.Synchronize(fromType, toType),
					    Expression.Convert(from, fromType),
					    Expression.Convert(to, toType)
				    ),
				    Expression.Goto(mappingEnd)
			    )
		    );
	    }
    }
}
