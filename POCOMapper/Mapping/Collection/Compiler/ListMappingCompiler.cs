using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.MappingCompilaton;

namespace KST.POCOMapper.Mapping.Collection.Compiler
{
    public class ListMappingCompiler<TFrom, TTo> : CollectionMappingCompiler<TFrom, TTo>
    {
	    public ListMappingCompiler(IUnresolvedMapping itemMapping, Delegate childPostprocessing, bool mapNullToEmpty)
		    : base(itemMapping, childPostprocessing, mapNullToEmpty)
	    {
	    }

	    protected override Expression CreateCollectionInstantiationExpression(Expression itemMappingExpression)
	    {
		    return Expression.Call(null, LinqMethods.ToList(EnumerableReflection<TTo>.ItemType), itemMappingExpression);
	    }

	    protected override Expression CreateEmptyCollectionExpression()
	    {
		    return Expression.New(ListMappingCompiler<TFrom, TTo>.GetDefaultConstructor());
	    }

	    private static ConstructorInfo GetDefaultConstructor()
	    {
		    ConstructorInfo constructTo = typeof(List<>)
			    .MakeGenericType(EnumerableReflection<TTo>.ItemType)
			    .GetConstructor(
				    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
				    null,
				    new Type[] {},
				    null
			    );
		    return constructTo;
	    }

	    public static bool ShouldUse()
	    {
		    return typeof(TTo).IsGenericType
		           && (typeof(TTo).GetGenericTypeDefinition() == typeof(List<>) || typeof(TTo).GetGenericTypeDefinition() == typeof(ICollection<>));
	    }
    }
}
