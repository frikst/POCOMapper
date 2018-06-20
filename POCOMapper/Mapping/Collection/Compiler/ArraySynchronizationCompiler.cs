using System;
using System.Linq.Expressions;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.MappingCompilaton;

namespace KST.POCOMapper.Mapping.Collection.Compiler
{
    internal class ArraySynchronizationCompiler<TFrom, TTo> : CollectionSynchronizationCompiler<TFrom, TTo>
    {
	    public ArraySynchronizationCompiler(IMapping itemMapping, Delegate selectIdFrom, Delegate selectIdTo, Delegate childPostprocessing)
		    : base(itemMapping, selectIdFrom, selectIdTo, childPostprocessing)
	    {
	    }

	    protected override Expression<Func<TFrom, TTo, TTo>> CompileToExpression()
	    {
		    ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");
		    ParameterExpression to = Expression.Parameter(typeof(TTo), "to");

		    return this.CreateSynchronizationEnvelope(
			    from, to,
			    Expression.Call(null, LinqMethods.ToArray(EnumerableReflection<TTo>.ItemType),
				    this.CreateItemSynchronizationExpression(from, to)
			    )
		    );
	    }
    }
}
