using System;

namespace KST.POCOMapper.Mapping.MappingCompilaton
{
    public abstract class ComparisionCompiler<TFrom, TTo> : ExpressionCompiler<Func<TFrom, TTo, bool>>
    {
        public bool MapEqual(TFrom from, TTo to)
        {
            var comparisionFunc = this.EnsureCompiled();
            return comparisionFunc(from, to);
        }
    }
}
