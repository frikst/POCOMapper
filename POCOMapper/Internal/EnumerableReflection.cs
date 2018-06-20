using System;
using System.Collections.Generic;
using System.Linq;

namespace KST.POCOMapper.Internal
{
    internal static class EnumerableReflection<TEnumerable>
    {
	    public static Type ItemType
	    {
		    get
		    {
			    if (typeof(TEnumerable).IsArray)
				    return typeof(TEnumerable).GetElementType();
			    else if (typeof(TEnumerable).IsGenericType && typeof(TEnumerable).GetGenericTypeDefinition() == typeof(IEnumerable<>))
				    return typeof(TEnumerable).GetGenericArguments()[0];
			    else
				    return typeof(TEnumerable).GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)).GetGenericArguments()[0];
		    }
	    }
    }
}
