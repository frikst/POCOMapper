using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KST.POCOMapper.Internal
{
    internal static class BasicNetTypes
    {
	    private static readonly Dictionary<Type, Type[]> aPrimitiveTypeImplicitConversions = new Dictionary<Type, Type[]>
	    {
		    {typeof(sbyte), new[] {typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(byte), new[] {typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(short), new[] {typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(ushort), new[] {typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(int), new[] {typeof(long), typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(uint), new[] {typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(long), new[] {typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(ulong), new[] {typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(char), new[] {typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(float), new[] {typeof(double)}},
		    {typeof(double), new Type[0]},
		    {typeof(decimal), new Type[0]},
		    {typeof(bool), new Type[0]},
	    };

	    private static readonly Type[] aPrimitiveLikeTypes = new[]
	    {
		    typeof(DateTime),
		    typeof(DateTimeOffset),
		    typeof(Guid),
			typeof(TimeSpan),
			typeof(string),
	    };

	    public static IEnumerable<Type> GetPrimitiveTypes()
		    => BasicNetTypes.aPrimitiveTypeImplicitConversions.Keys;

	    public static IEnumerable<Type> GetPrimitiveLikeTypes()
		    => BasicNetTypes.aPrimitiveLikeTypes;

	    public static IEnumerable<Type> GetImplicitTypeConversions(Type from)
	    {
		    if (BasicNetTypes.aPrimitiveTypeImplicitConversions.TryGetValue(from, out var ret))
			    return ret;

		    return Enumerable.Empty<Type>();
	    }
    }
}
