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
	    public ArrayMappingCompiler(IUnresolvedMapping itemMapping, Delegate childPostprocessing, bool mapNullToEmpty)
		    : base(itemMapping, childPostprocessing, mapNullToEmpty)
	    {
	    }

	    protected override Expression CreateCollectionInstantiationExpression(Expression itemMappingExpression)
	    {
		    return Expression.Call(null, LinqMethods.ToArray(EnumerableReflection<TTo>.ItemType), itemMappingExpression);
	    }

	    protected override Expression CreateEmptyCollectionExpression()
	    {
		    return Expression.Constant(Array.CreateInstance(EnumerableReflection<TTo>.ItemType, 0), typeof(TTo));
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
