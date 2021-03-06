﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Mapping.MappingCompilaton;
using KST.POCOMapper.Mapping.Object.Parser;

namespace KST.POCOMapper.Mapping.Object.Compilers
{
    internal class ObjectToObjectMappingCompiler<TFrom, TTo> : MappingCompiler<TFrom, TTo>
    {
	    private readonly Func<TFrom, TTo> aFactoryFunction;
	    private readonly IEnumerable<PairedMembers> aMemberPairs;
	    private readonly MappingDefinitionInformation aMappingDefinition;

	    public ObjectToObjectMappingCompiler(MappingDefinitionInformation mappingDefinition, Func<TFrom, TTo> factoryFunction, IEnumerable<PairedMembers> memberPairs)
	    {
		    this.aMemberPairs = memberPairs;
		    this.aMappingDefinition = mappingDefinition;
		    this.aFactoryFunction = factoryFunction;
	    }

	    protected override Expression<Func<TFrom, TTo>> CompileToExpression()
	    {
		    var from = Expression.Parameter(typeof(TFrom), "from");
		    var to = Expression.Parameter(typeof(TTo), "to");

		    var pairedMembers = this.aMemberPairs.ToList();

		    var temporaryVariables = new TemporaryVariables(pairedMembers, from, to);

		    Expression body = Expression.Block(
			    new ParameterExpression[] { to }.Concat(temporaryVariables.Variables),

			    Expression.Assign(
				    to,
				    this.NewExpression(from, typeof(TTo))
			    ),
			    this.MakeBlock(
				    temporaryVariables.InitialAssignments
			    ),
			    this.MakeBlock(
				    pairedMembers.Select(
					    x => x.CreateMappingAssignmentExpression(
						    x.From.Parent == null ? from : temporaryVariables[x.From.Parent],
						    x.To.Parent == null ? to : temporaryVariables[x.To.Parent],
						    this.aMappingDefinition.GetChildPostprocessing(typeof(TTo), x.To.Type),
						    to
					    )
				    )
			    ),
			    this.MakeBlock(
				    temporaryVariables.FinalAssignments
			    ),
			    to
		    );

		    return Expression.Lambda<Func<TFrom, TTo>>(
			    body,
			    from
		    );
	    }

		private Expression MakeBlock(IEnumerable<Expression> expressions)
		{
			var retExpressions = expressions.ToArray();

			if (retExpressions.Length == 0)
				return Expression.Empty();

			return Expression.Block(retExpressions);
		}

		private Expression NewExpression(Expression from, Type type)
		{
			if (this.aFactoryFunction != null)
				return ExpressionHelper.Call(this.aFactoryFunction, from);

			var constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { }, null);

			if (constructor == null)
				throw new InvalidMappingException($"Cannot find constructor for type {typeof(TTo).FullName}");

			return Expression.New(constructor);
		}
    }
}
