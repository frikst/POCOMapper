using System;
using System.Linq.Expressions;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.MappingCompilaton;
using KST.POCOMapper.SpecialRules;

namespace KST.POCOMapper.Mapping.Collection.Compiler
{
    internal class ArraySynchronizationCompiler<TFrom, TTo> : CollectionSynchronizationCompiler<TFrom, TTo>
    {
	    public ArraySynchronizationCompiler(IUnresolvedMapping itemMapping, IEqualityRules equalityRules, Delegate childPostprocessing, bool mapNullToEmpty)
		    : base(itemMapping, equalityRules, childPostprocessing, mapNullToEmpty)
	    {
	    }
		
	    protected override Expression CreateCollectionInstantiationExpression(Expression itemSynchronizationExpression)
	    {
		    return Expression.Call(null, LinqMethods.ToArray(EnumerableReflection<TTo>.ItemType), itemSynchronizationExpression);
	    }

	    protected override Expression CreateEmptyCollectionExpression()
	    {
		    return Expression.Constant(Array.CreateInstance(EnumerableReflection<TTo>.ItemType, 0), typeof(TTo));
	    }
    }
}
