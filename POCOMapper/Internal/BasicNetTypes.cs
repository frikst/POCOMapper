using System;
using System.Collections.Generic;
using System.Linq;

namespace KST.POCOMapper.Internal
{
    internal static class BasicNetTypes
    {
	    private static readonly Dictionary<Type, HashSet<Type>> aImplicitTypeConversions = new Dictionary<Type, HashSet<Type>>
	    {
		    {typeof(sbyte), new HashSet<Type> {typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(byte), new HashSet<Type> {typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(short), new HashSet<Type> {typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(ushort), new HashSet<Type> {typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(int), new HashSet<Type> {typeof(long), typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(uint), new HashSet<Type> {typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(long), new HashSet<Type> {typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(ulong), new HashSet<Type> {typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(char), new HashSet<Type> {typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal)}},
		    {typeof(float), new HashSet<Type> {typeof(double)}},
	    };

	    private static readonly Dictionary<Type, HashSet<Type>> aExplicitTypeConversions = new Dictionary<Type, HashSet<Type>>
	    {
		    {typeof(sbyte), new HashSet<Type> {typeof(byte), typeof(ushort), typeof(uint), typeof(ulong), typeof(char)}},
		    {typeof(byte), new HashSet<Type> {typeof(sbyte), typeof(char)}},
		    {typeof(short), new HashSet<Type> {typeof(sbyte), typeof(byte), typeof(ushort), typeof(uint), typeof(ulong), typeof(char)}},
		    {typeof(ushort), new HashSet<Type> {typeof(sbyte), typeof(byte), typeof(short), typeof(char)}},
		    {typeof(int), new HashSet<Type> {typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(uint), typeof(ulong), typeof(char)}},
		    {typeof(uint), new HashSet<Type> {typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(char)}},
		    {typeof(long), new HashSet<Type> {typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(ulong), typeof(char)}},
		    {typeof(ulong), new HashSet<Type> {typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(char)}},
		    {typeof(char), new HashSet<Type> {typeof(sbyte), typeof(byte), typeof(short)}},
		    {typeof(float), new HashSet<Type> {typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(char), typeof(decimal)}},
		    {typeof(double), new HashSet<Type> {typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(char), typeof(float), typeof(decimal)}},
		    {typeof(decimal), new HashSet<Type> {typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(char), typeof(float), typeof(double)}},
	    };

	    private static readonly Type[] aPrimitiveTypes = {
		    typeof(sbyte),
		    typeof(byte),
		    typeof(short),
		    typeof(ushort),
		    typeof(int),
		    typeof(uint),
		    typeof(long),
		    typeof(ulong),
		    typeof(char),
		    typeof(float),
		    typeof(double),
		    typeof(bool),
	    };

	    private static readonly Type[] aPrimitiveLikeTypes = {
		    typeof(decimal),
		    typeof(string),
		    typeof(DateTime),
		    typeof(DateTimeOffset),
		    typeof(Guid),
			typeof(TimeSpan),
	    };

	    public static IEnumerable<Type> GetPrimitiveTypes()
		    => BasicNetTypes.aPrimitiveTypes;

	    public static IEnumerable<Type> GetPrimitiveLikeTypes()
		    => BasicNetTypes.aPrimitiveLikeTypes;

	    public static IEnumerable<Type> GetImplicitTypeConversions(Type from)
	    {
		    if (BasicNetTypes.aImplicitTypeConversions.TryGetValue(from, out var ret))
			    return ret;

		    return Enumerable.Empty<Type>();
	    }

	    public static bool IsCastable<TFrom, TTo>()
		    => BasicNetTypes.IsCastable(typeof(TFrom), typeof(TTo));

	    public static bool IsCastable(Type from, Type to)
	    {
		    return BasicNetTypes.IsImplicitlyCastable(from, to) || BasicNetTypes.IsExplicitlyCastable(from, to);
	    }

	    public static bool IsImplicitlyCastable(Type from, Type to)
	    {
		    if (BasicNetTypes.aImplicitTypeConversions.TryGetValue(from, out var ret))
			    return ret.Contains(to);

		    return false;
	    }

	    public static bool IsExplicitlyCastable(Type from, Type to)
	    {
		    var innerFrom = from.IsEnum ? Enum.GetUnderlyingType(from) : from;
		    var innerTo = to.IsEnum ? Enum.GetUnderlyingType(to) : to;

		    if (BasicNetTypes.aExplicitTypeConversions.TryGetValue(innerFrom, out var ret))
			    return ret.Contains(innerTo);

		    return false;
	    }
    }
}
