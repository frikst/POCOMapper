using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.MappingCompilaton;

namespace KST.POCOMapper.Mapping.Collection.Compiler
{
    public class ListMappingCompiler<TFrom, TTo> : CollectionMappingCompiler<TFrom, TTo>
    {
	    public ListMappingCompiler(IUnresolvedMapping itemMapping, Delegate childPostprocessing)
		    : base(itemMapping, childPostprocessing)
	    {
	    }

	    protected override Expression<Func<TFrom, TTo>> CompileToExpression()
	    {
		    var from = Expression.Parameter(typeof(TFrom), "from");

		    return this.CreateMappingEnvelope(
			    from,
			    Expression.Call(null, LinqMethods.ToList(EnumerableReflection<TTo>.ItemType),
				    this.CreateItemMappingExpression(from)
			    )
		    );
	    }

	    public static bool ShouldUse()
	    {
		    return typeof(TTo).IsGenericType
		           && (typeof(TTo).GetGenericTypeDefinition() == typeof(List<>) || typeof(TTo).GetGenericTypeDefinition() == typeof(ICollection<>));
	    }
    }
}
