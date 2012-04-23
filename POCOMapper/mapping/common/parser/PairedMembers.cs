using System;
using System.Linq.Expressions;
using POCOMapper.conventions.members;
using POCOMapper.exceptions;
using POCOMapper.@internal;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.common.parser
{
	public class PairedMembers
	{
		public enum Action
		{
			Sync,
			Map
		}

		private readonly IMember aFrom;
		private readonly IMember aTo;
		private readonly IMapping aMapping;

		public PairedMembers(IMember from, IMember to, IMapping mapping)
		{
			aFrom = from;
			aTo = to;
			aMapping = mapping;
		}

		public IMember From
		{
			get { return aFrom; }
		}

		public IMember To
		{
			get { return aTo; }
		}

		public IMapping Mapping
		{
			get { return aMapping; }
		}

		public Expression CreateAssignmentExpression(ParameterExpression from, ParameterExpression to, Action action, Delegate postprocess, ParameterExpression parent)
		{
			if (
				(
					action == Action.Map
					|| this.aTo.Type.IsValueType
					|| this.aTo.Type.IsAssignableFrom(typeof(string))
					|| (this.aMapping != null && !this.aMapping.CanSynchronize)
				) && (
					this.aMapping == null
					|| this.aMapping.CanMap
				))
			{
				Expression ret = this.aFrom.CreateGetterExpression(from);

				if (this.aMapping != null)
					ret = Expression.Call(
						Expression.Constant(this.aMapping),
						MappingMethods.Map(this.aFrom.Type, this.aTo.Type),
						ret
					);

				ret = this.aTo.CreateSetterExpression(
					to,
					ret
				);

				return this.AddPostprocess(ret, to, postprocess, parent);

			}
			else
			{
				ParameterExpression tempFromValue = Expression.Parameter(this.aFrom.Type, "tempFrom");
				ParameterExpression tempToValue = Expression.Parameter(this.aTo.Type, "tempTo");

				if (this.aTo.Getter == null)
					// TODO: ???
					throw new InvalidMapping("Cannot synchronize object with setter method mapping destination without any getter method defined");

				if (this.aMapping == null)
					// TODO: ???
					throw new InvalidMapping("Cannot synchronize two reference objects with the same type");

				Expression synchronize = Expression.Call(
					Expression.Constant(this.aMapping),
					MappingMethods.Synchronize(this.aFrom.Type, this.aTo.Type),
					tempFromValue, this.aTo.CreateGetterExpression(to)
				);

				if (this.aMapping.CanMap)
				{
					Expression map = this.aTo.CreateSetterExpression(
						to,
						Expression.Call(
							Expression.Constant(this.aMapping),
							MappingMethods.Map(this.aFrom.Type, this.aTo.Type),
							tempFromValue
						)
					);
					map = this.AddPostprocess(map, to, postprocess, parent);
					synchronize = Expression.IfThenElse(
						Expression.Equal(tempToValue, Expression.Constant(null)),
						map,
						synchronize
					);
				}

				if (this.aTo.Setter != null)
					synchronize = Expression.IfThenElse(
						Expression.Equal(tempFromValue, Expression.Constant(null)),
						this.aTo.CreateSetterExpression(to, Expression.Constant(null, this.aTo.Type)),
						synchronize
					);

				return Expression.Block(
					new ParameterExpression[] { tempFromValue, tempToValue },
					Expression.Assign(tempFromValue, this.aFrom.CreateGetterExpression(from)),
					Expression.Assign(tempToValue, this.aTo.CreateGetterExpression(to)),
					synchronize
				);
			}
		}

		private Expression AddPostprocess(Expression assignment, ParameterExpression to, Delegate postprocess, ParameterExpression parent)
		{
			if (postprocess != null)
			{
				Expression postprocessTarget = postprocess.Target == null ? null : Expression.Constant(postprocess.Target);
				return Expression.Block(
					new Expression[]
						{
							assignment,
							Expression.Call(postprocessTarget, postprocess.Method, parent, this.aTo.CreateGetterExpression(to))
						}
				);
			}
			else
			{
				return assignment;
			}
		}

		public override string ToString()
		{
			return string.Format("{0} => {1}", this.aFrom, this.aTo);
		}
	}
}
