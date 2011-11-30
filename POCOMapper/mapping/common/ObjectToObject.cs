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
		private readonly IEnumerable<PairedMembers> aMemberPairs;

		public ObjectToObject(MappingImplementation mapping)
			: base(mapping)
		{
			this.aMemberPairs = new TypePairParser(
				this.Mapping,
				typeof(TFrom),
				typeof(TTo)
			).ToList();
		}

		#region Overrides of CompiledMapping<TFrom,TTo>

		public override IEnumerable<Tuple<string, IMapping>> Children
		{
			get
			{
				foreach (var mapping in this.aMemberPairs)
					yield return new Tuple<string, IMapping>(string.Format("{0} => {1}", mapping.From.Getter.Name, mapping.To.Setter.Name), mapping.Mapping);
			}
		}

		protected override Expression<Func<TFrom, TTo>> CompileMapping()
		{
			ConstructorInfo constructor = typeof(TTo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { }, null);

			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");
			ParameterExpression to = Expression.Parameter(typeof(TTo), "to");

			Dictionary<IMember, ParameterExpression> temporaryVariables = new Dictionary<IMember, ParameterExpression>();
			List<Expression> assignments = new List<Expression>();

			foreach (PairedMembers memberPair in aMemberPairs)
				foreach (IMember parent in GetAllMemberParents(memberPair, temporaryVariables))
					assignments.Add(
						Expression.Assign(
							temporaryVariables[parent],
							parent.CreateGetterExpression(parent.Parent == null ? from : temporaryVariables[parent.Parent])
						)
					);

			// TODO: synchronization with structuring
			return Expression.Lambda<Func<TFrom, TTo>>(
				Expression.Block(
					new ParameterExpression[] { to }.Concat(temporaryVariables.Select(x => x.Value)),

					Expression.Assign(
						to,
						Expression.New(constructor)
					),
					this.MakeBlock(
						assignments
					),
					this.MakeBlock(
						this.aMemberPairs.Select(
							x => x.CreateAssignmentExpression(
								x.From.Parent == null ? from : temporaryVariables[x.From.Parent],
								to,
								PairedMembers.Action.Map
							)
						)
					),
					to
				),
				from
			);
		}

		private IEnumerable<IMember> GetAllMemberParents(PairedMembers memberPair, Dictionary<IMember, ParameterExpression> temporaryVariables)
		{
			List<IMember> allParents = new List<IMember>();

			IMember parent = memberPair.From.Parent;

			while (parent != null && !temporaryVariables.ContainsKey(parent))
			{
				allParents.Insert(0, parent);
				temporaryVariables[parent] = Expression.Parameter(parent.Type, string.Format("tmp{0}", temporaryVariables.Count));

				parent = parent.Parent;
			}
			return allParents;
		}

		protected override Expression<Action<TFrom, TTo>> CompileSynchronization()
		{
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");
			ParameterExpression to = Expression.Parameter(typeof(TTo), "to");

			// TODO: synchronization with flattering and structuring
			return Expression.Lambda<Action<TFrom, TTo>>(
				this.MakeBlock(
					this.aMemberPairs.Select(x => x.CreateAssignmentExpression(from, to, PairedMembers.Action.Sync))
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

		#endregion
	}
}
