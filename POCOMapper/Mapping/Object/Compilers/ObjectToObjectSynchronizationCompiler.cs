using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.MappingCompilaton;
using KST.POCOMapper.Mapping.Object.Parser;

namespace KST.POCOMapper.Mapping.Object.Compilers
{
    internal class ObjectToObjectSynchronizationCompiler<TFrom, TTo> : SynchronizationCompiler<TFrom, TTo>
    {
	    private readonly IEnumerable<PairedMembers> aMemberPairs;
	    private readonly MappingImplementation aMapping;

	    public ObjectToObjectSynchronizationCompiler(MappingImplementation mapping, IEnumerable<PairedMembers> memberPairs)
	    {
		    this.aMemberPairs = memberPairs;
		    this.aMapping = mapping;
	    }

	    protected override Expression<Func<TFrom, TTo, TTo>> CompileToExpression()
	    {
		    var from = Expression.Parameter(typeof(TFrom), "from");
		    var to = Expression.Parameter(typeof(TTo), "to");

		    var temporaryVariables = new TemporaryVariables(this.aMemberPairs, from, to);

		    return Expression.Lambda<Func<TFrom, TTo, TTo>>(
			    Expression.Block(
				    temporaryVariables.Variables,

				    this.MakeBlock(
					    temporaryVariables.InitialAssignments
				    ),
				    this.MakeBlock(
					    this.aMemberPairs.Select(
						    x => x.CreateSynchronizationAssignmentExpression(
							    x.From.Parent == null ? from : temporaryVariables[x.From.Parent],
							    x.To.Parent == null ? to : temporaryVariables[x.To.Parent],
							    this.aMapping.GetChildPostprocessing(typeof(TTo), x.To.Type),
							    to
						    )
					    )
				    ),
				    this.MakeBlock(
					    temporaryVariables.FinalAssignments
				    ),
				    to
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
