using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using KST.POCOMapper.Mapping.Object.Parser;
using KST.POCOMapper.Members;

namespace KST.POCOMapper.Mapping.Object.Compilers
{
	internal class TemporaryVariables
	{
		private readonly Dictionary<IMember, ParameterExpression> aTemporaryVariables;

		public TemporaryVariables(IEnumerable<PairedMembers> memberPairs, ParameterExpression from, ParameterExpression to)
		{
			this.aTemporaryVariables = new Dictionary<IMember, ParameterExpression>();
			this.InitialAssignments = new List<Expression>();
			this.FinalAssignments = new List<Expression>();

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
							TemporaryVariables.BetterCoalesce(
								parent.CreateGetterExpression(parentVariable),
								TemporaryVariables.NewExpression(parent.Type)
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
			=> this.aTemporaryVariables.Values;

		public List<Expression> InitialAssignments { get; }
		public List<Expression> FinalAssignments { get; }

		public ParameterExpression this[IMember member]
			=> this.aTemporaryVariables[member];

		private IEnumerable<IMember> GetAllMemberParents(IMember member, IDictionary<IMember, ParameterExpression> temporaryVariables)
		{
			List<IMember> allParents = new List<IMember>();

			IMember parent = member.Parent;

			while (parent != null && !temporaryVariables.ContainsKey(parent))
			{
				allParents.Insert(0, parent);
				temporaryVariables[parent] = Expression.Parameter(parent.Type, $"tmp{temporaryVariables.Count}");

				parent = parent.Parent;
			}
			return allParents;
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

		private static Expression NewExpression(Type type)
		{
			var constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { }, null);

			if (constructor == null)
				return null;

			return Expression.New(constructor);
		}
	}
}
