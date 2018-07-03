using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.MappingCompilaton;

namespace KST.POCOMapper.Mapping.Collection.Compiler
{
    internal class ArrayMappingCompiler<TFrom, TTo> : CollectionMappingCompiler<TFrom, TTo>
    {
	    public ArrayMappingCompiler(IUnresolvedMapping itemMapping, Delegate childPostprocessing)
		    : base(itemMapping, childPostprocessing)
	    {
	    }

	    protected override Expression<Func<TFrom, TTo>> CompileToExpression()
	    {
		    var from = Expression.Parameter(typeof(TFrom), "from");

		    return this.CreateMappingEnvelope(
			    from,
			    Expression.Call(null, LinqMethods.ToArray(EnumerableReflection<TTo>.ItemType),
				    this.CreateItemMappingExpression(from)
			    )
		    );
	    }

	    public static bool ShouldUse()
	    {
		    if (typeof(TTo).IsArray)
			    return true;

		    if (typeof(TTo).IsGenericType && typeof(TTo).GetGenericTypeDefinition() == typeof(IEnumerable<>))
			    return true;

		    return false;
	    }
    }
}
