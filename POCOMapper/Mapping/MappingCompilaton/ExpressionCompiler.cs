using System.Linq.Expressions;
using KST.POCOMapper.Internal;

namespace KST.POCOMapper.Mapping.MappingCompilaton
{
    public abstract class ExpressionCompiler<TDelegate>
	    where TDelegate : class
    {
	    private TDelegate aFnc;

	    protected ExpressionCompiler()
	    {
		    this.aFnc = null;
	    }

	    public string Source { get; private set; }


	    protected TDelegate EnsureCompiled()
	    {
		    if (this.aFnc != null)
			    return this.aFnc;

		    var expression = this.CompileToExpression();
		    this.Source = ExpressionHelper.GetDebugView(expression);
		    this.aFnc = expression.Compile();
		    return this.aFnc;
	    }

	    protected abstract Expression<TDelegate> CompileToExpression();
    }
}
