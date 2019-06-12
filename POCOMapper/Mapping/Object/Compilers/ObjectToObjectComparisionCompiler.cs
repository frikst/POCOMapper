using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.MappingCompilaton;
using KST.POCOMapper.Mapping.Object.Parser;

namespace KST.POCOMapper.Mapping.Object.Compilers
{
    internal class ObjectToObjectComparisionCompiler<TFrom, TTo> : ComparisionCompiler<TFrom, TTo>
    {
	    private readonly IEnumerable<PairedMembers> aMemberPairs;
	    private readonly MappingDefinitionInformation aMappingDefinition;

	    public ObjectToObjectComparisionCompiler(MappingDefinitionInformation mappingDefinition, IEnumerable<PairedMembers> memberPairs)
	    {
		    this.aMemberPairs = memberPairs;
		    this.aMappingDefinition = mappingDefinition;
	    }

	    protected override Expression<Func<TFrom, TTo, bool>> CompileToExpression()
	    {
		    var from = Expression.Parameter(typeof(TFrom), "from");
		    var to = Expression.Parameter(typeof(TTo), "to");

            var end = Expression.Label(typeof(bool));

            var pairedMembers = this.aMemberPairs.ToList();

            var temporaryVariables = new TemporaryVariables(pairedMembers, from, to);

		    return Expression.Lambda<Func<TFrom, TTo, bool>>(
			    Expression.Block(
                    temporaryVariables.Variables,
                    this.MakeBlock(temporaryVariables.InitialAssignments),
				    this.MakeBlock(
					    pairedMembers.Select(
						    x => x.CreateComparisionExpression(
							    x.From.Parent == null ? from : temporaryVariables[x.From.Parent],
							    x.To.Parent == null ? to : temporaryVariables[x.To.Parent],
                                end
						    )
					    )
				    ),
				    Expression.Label(end, Expression.Constant(true))
			    ),
			    from, to
		    );
	    }

	    private Expression MakeBlock(IEnumerable<Expression> expressions)
	    {
		    var retExpressions = expressions.ToArray();

		    if (retExpressions.Length == 0)
			    return Expression.Empty();

		    return Expression.Block(retExpressions);
	    }
    }
}
