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

#if DEBUG
	    public string Source { get; private set; }
#endif

	    protected TDelegate EnsureCompiled()
	    {
		    if (this.aFnc != null)
			    return this.aFnc;

		    var expression = this.CompileToExpression();
#if DEBUG
		    this.Source = ExpressionHelper.GetDebugView(expression);
#endif
		    this.aFnc = expression.Compile();
		    return this.aFnc;
	    }

	    protected abstract Expression<TDelegate> CompileToExpression();
    }
}
