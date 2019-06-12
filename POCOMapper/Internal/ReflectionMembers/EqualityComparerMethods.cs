using System;
using System.Collections.Generic;
using System.Reflection;

namespace KST.POCOMapper.Internal.ReflectionMembers
{
    internal static class EqualityComparerMethods
    {
        public static MethodInfo Equals(Type type)
        {
            return typeof(IEqualityComparer<>).MakeGenericType(type).GetMethod(nameof(IEqualityComparer<object>.Equals));
        }

        public static object GetEqualityComparer(Type type)
        {
            return typeof(EqualityComparer<>).MakeGenericType(type)
                .GetProperty(nameof(EqualityComparer<object>.Default), BindingFlags.Static | BindingFlags.Public)
                .GetValue(null, null);
        }
    }
}
