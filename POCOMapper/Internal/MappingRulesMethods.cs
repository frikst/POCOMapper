using System;
using System.Reflection;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Internal
{
    internal static class MappingRulesMethods
    {
	    public static MethodInfo GetCreate(Type from, Type to)
	    {
		    return typeof(IMappingRules).GetMethod(nameof(IMappingRules.Create)).MakeGenericMethod(from, to);
	    }

    }
}
