using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.MappingCompilaton;

namespace KST.POCOMapper.Mapping.Collection.Compiler
{
    internal class EnumerableMappingCompiler<TFrom, TTo> : CollectionMappingCompiler<TFrom, TTo>
    {
	    public EnumerableMappingCompiler(IUnresolvedMapping itemMapping, Delegate childPostprocessing)
		    : base(itemMapping, childPostprocessing)
	    {
	    }

	    protected override Expression<Func<TFrom, TTo>> CompileToExpression()
	    {
		    ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");

		    if (typeof(TTo).IsGenericType && typeof(TTo).GetGenericTypeDefinition() == typeof(IEnumerable<>))
		    {
			    return this.CreateMappingEnvelope(
				    from,
				    this.CreateItemMappingExpression(from)
			    );
		    }
		    else
		    {
			    ConstructorInfo constructTo = typeof(TTo).GetConstructor(
				    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
				    null,
				    new Type[] {typeof(IEnumerable<>).MakeGenericType(EnumerableReflection<TTo>.ItemType)},
				    null
			    );

			    if (constructTo == null)
				    throw new InvalidMappingException($"Cannot find constructor for type {typeof(TTo).FullName}");

			    return this.CreateMappingEnvelope(
				    from,
				    Expression.New(constructTo,
					    this.CreateItemMappingExpression(from)
				    )
			    );
		    }
	    }
    }
}
