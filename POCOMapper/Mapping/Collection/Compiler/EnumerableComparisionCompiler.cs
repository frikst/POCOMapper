using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.MappingCompilaton;
using KST.POCOMapper.SpecialRules;

namespace KST.POCOMapper.Mapping.Collection.Compiler
{
    internal class EnumerableComparisionCompiler<TFrom, TTo> : ComparisionCompiler<TFrom, TTo>
    {
        private readonly IUnresolvedMapping aItemMapping;
        private readonly IEqualityRules aEqualityRules;

        public EnumerableComparisionCompiler(IUnresolvedMapping itemMapping, IEqualityRules equalityRules)
        {
            this.aItemMapping = itemMapping;
            this.aEqualityRules = equalityRules;
        }

        protected override Expression<Func<TFrom, TTo, bool>> CompileToExpression()
        {
            var from = Expression.Parameter(typeof(IEnumerable<>).MakeGenericType(EnumerableReflection<TFrom>.ItemType), "from");
            var to = Expression.Parameter(typeof(IEnumerable<>).MakeGenericType(EnumerableReflection<TTo>.ItemType), "to");

            var fromEnumeratorVariable = Expression.Parameter(typeof(IEnumerator<>).MakeGenericType(EnumerableReflection<TFrom>.ItemType), "fromEnumerator");
            var toEnumeratorVariable = Expression.Parameter(typeof(IEnumerator<>).MakeGenericType(EnumerableReflection<TTo>.ItemType), "toEnumerator");

            var hasFromVariable = Expression.Parameter(typeof(bool), "hasFrom");
            var hasToVariable = Expression.Parameter(typeof(bool), "hasTo");

            Expression itemsAreMapEqual;

            var fromCurrent = Expression.Property(fromEnumeratorVariable, EnumerableMethods.Current(EnumerableReflection<TFrom>.ItemType));
            var toCurrent = Expression.Property(toEnumeratorVariable, EnumerableMethods.Current(EnumerableReflection<TTo>.ItemType));

            if (this.aItemMapping.ResolvedMapping is IMappingWithSpecialComparision)
            {
                itemsAreMapEqual = Expression.Call(
                    Expression.Constant(this.aItemMapping.ResolvedMapping),
                    MappingMethods.MapEqual(EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType),
                    fromCurrent,
                    toCurrent
                );
            }
            else
            {
                Expression fromMapped = fromCurrent;

                if (!(this.aItemMapping.ResolvedMapping is IDirectMapping))
                {
                    fromMapped = Expression.Call(
                        Expression.Constant(this.aItemMapping.ResolvedMapping),
                        MappingMethods.Map(EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType),
                        fromMapped
                    );
                }

                itemsAreMapEqual = Expression.Call(
                    Expression.Constant(EqualityComparerMethods.GetEqualityComparer(EnumerableReflection<TTo>.ItemType)),
                    EqualityComparerMethods.Equals(EnumerableReflection<TTo>.ItemType),
                    fromMapped,
                    toCurrent
                );
            }

            if (this.aEqualityRules != null)
            {
                var (selectIdFrom, selectIdTo) = this.aEqualityRules.GetIdSelectors();
                itemsAreMapEqual = Expression.AndAlso(
                    Expression.Equal(
                        ExpressionHelper.Call(
                            selectIdFrom,
                            fromCurrent
                        ),
                        ExpressionHelper.Call(
                            selectIdTo,
                            toCurrent
                        )
                    ), 
                    itemsAreMapEqual
                );
            }

            var end = Expression.Label(typeof(bool));

            return Expression.Lambda<Func<TFrom, TTo, bool>>(
                Expression.Block(
                    new[] {fromEnumeratorVariable, toEnumeratorVariable, hasFromVariable, hasToVariable},
                    Expression.Assign(fromEnumeratorVariable, Expression.Call(from, EnumerableMethods.GetEnumerable(EnumerableReflection<TFrom>.ItemType))),
                    Expression.Assign(toEnumeratorVariable, Expression.Call(to, EnumerableMethods.GetEnumerable(EnumerableReflection<TTo>.ItemType))),

                    Expression.Loop(
                        Expression.Block(
                            Expression.Assign(hasFromVariable, Expression.Call(fromEnumeratorVariable, EnumerableMethods.MoveNext(EnumerableReflection<TFrom>.ItemType))),
                            Expression.Assign(hasToVariable, Expression.Call(toEnumeratorVariable, EnumerableMethods.MoveNext(EnumerableReflection<TTo>.ItemType))),

                            Expression.IfThen(
                                Expression.AndAlso(
                                    Expression.Not(hasFromVariable), 
                                    Expression.Not(hasToVariable)
                                ), 
                                Expression.Return(end, Expression.Constant(true))
                            ),

                            Expression.IfThen(
                                Expression.OrElse(
                                    Expression.Not(hasFromVariable), 
                                    Expression.Not(hasToVariable)
                                ), 
                                Expression.Return(end, Expression.Constant(false))
                            ),

                            Expression.IfThen(
                                Expression.Not(itemsAreMapEqual),
                                Expression.Return(end, Expression.Constant(false))
                            )
                        )
                    ),
                    Expression.Label(end, Expression.Constant(true))
                ),
                from, to
            );
        }
    }
}
