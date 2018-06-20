using System;

namespace KST.POCOMapper.Mapping.MappingCompilaton
{
    public abstract class SynchronizationCompiler<TFrom, TTo> : ExpressionCompiler<Func<TFrom, TTo, TTo>>
    {
	    public TTo Synchronize(TFrom from, TTo to)
	    {
		    var synchronizeFnc = this.EnsureCompiled();
		    return synchronizeFnc(from, to);
	    }
    }
}
