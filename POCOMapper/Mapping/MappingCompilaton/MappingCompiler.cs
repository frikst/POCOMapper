using System;

namespace KST.POCOMapper.Mapping.MappingCompilaton
{
    public abstract class MappingCompiler<TFrom, TTo> : ExpressionCompiler<Func<TFrom, TTo>>
    {
	    public TTo Map(TFrom from)
	    {
			var mappingFnc = this.EnsureCompiled();
		    return mappingFnc(from);
	    }
    }
}
