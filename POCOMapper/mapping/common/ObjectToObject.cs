using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using POCOMapper.conventions.members;
using POCOMapper.definition;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.common.parser;

namespace POCOMapper.mapping.common
{
	public class ObjectToObject<TFrom, TTo> : CompiledMapping<TFrom, TTo>
	{
		private class TemporaryVariables
		{
			private Dictionary<IMember, ParameterExpression> aTemporaryVariables;

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
								Expression.Coalesce(
									parent.CreateGetterExpression(parentVariable),
									ObjectToObject<TFrom, TTo>.NewExpression(parent.Type)
								)
							)
						);
						this.FinalAssignments.Add(
							parent.CreateSetterExpression(
								parentVariable,
								this.aTemporaryVariables[parent]
							)
						);
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

		public ObjectToObject(MappingImplementation mapping, IEnumerable<PairedMembers> explicitPairs, bool implicitMappings)
			: base(mapping)
		{
			if (implicitMappings)
			{
				this.aMemberPairs = new TypePairParser(
					this.Mapping,
					typeof(TFrom),
					typeof(TTo)
				).Union(explicitPairs);
			}
			else
			{
				this.aMemberPairs = explicitPairs;
			}
		}

		#region Overrides of CompiledMapping<TFrom,TTo>

		public override IEnumerable<Tuple<string, IMapping>> Children
		{
			get
			{
				foreach (var mapping in this.aMemberPairs)
					yield return new Tuple<string, IMapping>(string.Format("{0} => {1}", mapping.From.Name, mapping.To.Name), mapping.Mapping);
			}
		}

		public override bool CanSynchronize
		{
			get { return true; }
		}

		public override bool CanMap
		{
			get { return true; }
		}

		protected override Expression<Func<TFrom, TTo>> CompileMapping()
		{
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");
			ParameterExpression to = Expression.Parameter(typeof(TTo), "to");

			TemporaryVariables temporaryVariables = new TemporaryVariables(this.aMemberPairs, from, to);

			return Expression.Lambda<Func<TFrom, TTo>>(
				Expression.Block(
					new ParameterExpression[] { to }.Concat(temporaryVariables.Variables),

					Expression.Assign(
						to,
						ObjectToObject<TFrom, TTo>.NewExpression(typeof(TTo))
					),
					this.MakeBlock(
						temporaryVariables.InitialAssignments
					),
					this.MakeBlock(
						this.aMemberPairs.Select(
							x => x.CreateAssignmentExpression(
								x.From.Parent == null ? from : temporaryVariables[x.From.Parent],
								x.To.Parent == null ? to : temporaryVariables[x.To.Parent],
								PairedMembers.Action.Map
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

		protected override Expression<Action<TFrom, TTo>> CompileSynchronization()
		{
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");
			ParameterExpression to = Expression.Parameter(typeof(TTo), "to");

			TemporaryVariables temporaryVariables = new TemporaryVariables(this.aMemberPairs, from, to);

			return Expression.Lambda<Action<TFrom, TTo>>(
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
								PairedMembers.Action.Sync
							)
						)
					),
					this.MakeBlock(
						temporaryVariables.FinalAssignments
					)
				),
				from, to
			);
		}

		private Expression MakeBlock(IEnumerable<Expression> expressions)
		{
			Expression[] retExpressions = expressions.ToArray();

			if (retExpressions.Length == 0)
				return Expression.Empty();

			return Expression.Block(retExpressions);
		}

		private static Expression NewExpression(Type type)
		{
			ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { }, null);

			return Expression.New(constructor);
		}

		#endregion
	}
}
