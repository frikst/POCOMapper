using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using POCOMapper.conventions.members;
using POCOMapper.conventions.symbol;
using POCOMapper.definition;
using POCOMapper.exceptions;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.common.parser;
using POCOMapper.visitor;

namespace POCOMapper.mapping.common
{
	public class ObjectToObject<TFrom, TTo> : CompiledMapping<TFrom, TTo>, IObjectMapping
	{
		private class TemporaryVariables
		{
			private readonly Dictionary<IMember, ParameterExpression> aTemporaryVariables;

			public TemporaryVariables(IEnumerable<PairedMembers> memberPairs, ParameterExpression from, ParameterExpression to)
			{
				aTemporaryVariables = new Dictionary<IMember, ParameterExpression>();
				InitialAssignments = new List<Expression>();
				FinalAssignments = new List<Expression>();

				foreach (PairedMembers memberPair in memberPairs)
				{
					foreach (IMember parent in this.GetAllMemberParents(memberPair.From, this.aTemporaryVariables))
					{
						ParameterExpression parentVariable = parent.Parent == null ? from : this.aTemporaryVariables[parent.Parent];
						this.InitialAssignments.Add(
							Expression.Assign(
								this.aTemporaryVariables[parent],
								parent.CreateGetterExpression(parentVariable)
							)
						);
					}
				}

				foreach (PairedMembers memberPair in memberPairs)
				{
					foreach (IMember parent in this.GetAllMemberParents(memberPair.To, this.aTemporaryVariables))
					{
						ParameterExpression parentVariable = parent.Parent == null ? to : this.aTemporaryVariables[parent.Parent];

						this.InitialAssignments.Add(
							Expression.Assign(
								this.aTemporaryVariables[parent],
								ObjectToObject<TFrom, TTo>.BetterCoalesce(
									parent.CreateGetterExpression(parentVariable),
									ObjectToObject<TFrom, TTo>.NewExpression(parent.Type)
								)
							)
						);

						Expression setterExpression = parent.CreateSetterExpression(
							parentVariable,
							this.aTemporaryVariables[parent]
						);
						if (setterExpression != null)
							this.FinalAssignments.Add(setterExpression);
					}
				}
			}

			public IEnumerable<ParameterExpression> Variables
			{
				get { return aTemporaryVariables.Values; }
			}

			public List<Expression> InitialAssignments { get; private set; }
			public List<Expression> FinalAssignments { get; private set; }

			public ParameterExpression this[IMember member]
			{
				get { return this.aTemporaryVariables[member]; }
			}

			private IEnumerable<IMember> GetAllMemberParents(IMember member, IDictionary<IMember, ParameterExpression> temporaryVariables)
			{
				List<IMember> allParents = new List<IMember>();

				IMember parent = member.Parent;

				while (parent != null && !temporaryVariables.ContainsKey(parent))
				{
					allParents.Insert(0, parent);
					temporaryVariables[parent] = Expression.Parameter(parent.Type, string.Format("tmp{0}", temporaryVariables.Count));

					parent = parent.Parent;
				}
				return allParents;
			}
		}

		private readonly IEnumerable<PairedMembers> aMemberPairs;
		private readonly Func<TFrom, TTo> aFactoryFunction;

		public ObjectToObject(Func<TFrom, TTo> factoryFunction, MappingImplementation mapping, IEnumerable<PairedMembers> explicitPairs, bool implicitMappings)
			: base(mapping)
		{
			this.aFactoryFunction = factoryFunction;

			if (implicitMappings)
			{
				List<PairedMembers> explicitPairsList = explicitPairs.ToList();
				HashSet<string> explicitSymbols = new HashSet<string>(explicitPairsList.Select(x => x.To.FullName));

				this.aMemberPairs = explicitPairsList.Concat(
					new TypePairParser(this.Mapping, typeof(TFrom), typeof(TTo))
						.Where(x => !explicitSymbols.Contains(x.To.FullName))
				);
			}
			else
			{
				this.aMemberPairs = explicitPairs;
			}
		}

		#region Overrides of CompiledMapping<TFrom,TTo>

		public override void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public override bool CanSynchronize
		{
			get { return true; }
		}

		public override bool CanMap
		{
			get { return true; }
		}

		public override bool IsDirect
		{
			get { return false; }
		}

		public override bool SynchronizeCanChangeObject
		{
			get { return false; }
		}

		protected override Expression<Func<TFrom, TTo>> CompileMapping()
		{
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");
			ParameterExpression to = Expression.Parameter(typeof(TTo), "to");

			List<PairedMembers> pairedMembers = this.aMemberPairs.ToList();

			TemporaryVariables temporaryVariables = new TemporaryVariables(pairedMembers, from, to);

			return Expression.Lambda<Func<TFrom, TTo>>(
				Expression.Block(
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
							x => x.CreateAssignmentExpression(
								x.From.Parent == null ? from : temporaryVariables[x.From.Parent],
								x.To.Parent == null ? to : temporaryVariables[x.To.Parent],
								PairedMembers.Action.Map,
								this.Mapping.GetChildPostprocessing(typeof(TTo), x.To.Type),
								to
							)
						)
					),
					this.MakeBlock(
						temporaryVariables.FinalAssignments
					),
					to
				),
				from
			);
		}

		protected override Expression<Func<TFrom, TTo, TTo>> CompileSynchronization()
		{
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");
			ParameterExpression to = Expression.Parameter(typeof(TTo), "to");

			TemporaryVariables temporaryVariables = new TemporaryVariables(this.aMemberPairs, from, to);

			return Expression.Lambda<Func<TFrom, TTo, TTo>>(
				Expression.Block(
					temporaryVariables.Variables,

					this.MakeBlock(
						temporaryVariables.InitialAssignments
					),
					this.MakeBlock(
						this.aMemberPairs.Select(
							x => x.CreateAssignmentExpression(
								x.From.Parent == null ? from : temporaryVariables[x.From.Parent],
								x.To.Parent == null ? to : temporaryVariables[x.To.Parent],
								PairedMembers.Action.Sync,
								this.Mapping.GetChildPostprocessing(typeof(TTo), x.To.Type),
								to
							)
						)
					),
					this.MakeBlock(
						temporaryVariables.FinalAssignments
					),
					to
				),
				from, to
			);
		}

		#endregion

		public IEnumerable<IObjectMemberMapping> Members
		{
			get
			{
				return this.aMemberPairs;
			}
		}

		private Expression MakeBlock(IEnumerable<Expression> expressions)
		{
			Expression[] retExpressions = expressions.ToArray();

			if (retExpressions.Length == 0)
				return Expression.Empty();

			return Expression.Block(retExpressions);
		}

		private Expression NewExpression(Expression from, Type type)
		{
			if (this.aFactoryFunction == null)
			{
				Expression newExpression = ObjectToObject<TFrom, TTo>.NewExpression(type);
				if (newExpression == null)
					throw new InvalidMapping(string.Format("Cannot find constructor for type {0}", typeof(TTo).FullName));
				return newExpression;
			}
			else
				return Expression.Invoke(Expression.Constant(this.aFactoryFunction), from);
		}

		private static Expression NewExpression(Type type)
		{
			ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { }, null);

			if (constructor == null)
				return null;

			return Expression.New(constructor);
		}

		private static Expression BetterCoalesce(Expression left, Expression right)
		{
			if (right == null && left == null)
				return Expression.Empty();
			else if (right == null)
				return left;
			else if (left == null)
				return right;
			else
				return Expression.Coalesce(left, right);
		}
	}
}
