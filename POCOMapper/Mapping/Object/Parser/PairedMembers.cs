using System;
using System.Linq.Expressions;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Members;

namespace KST.POCOMapper.Mapping.Object.Parser
{
	public class PairedMembers : IObjectMemberMapping
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
			if (!from.CanPairWith(to))
				throw new Exception($"Cannot map {from} and {to} together");

			this.aFrom = from;
			this.aTo = to;

			this.aMapping = mapping;
		}

		public IMember From
			=> this.aFrom;

		public IMember To
			=> this.aTo;

		public IMapping Mapping
			=> this.aMapping;

		public Expression CreateAssignmentExpression(ParameterExpression from, ParameterExpression to, Action action, Delegate postprocess, ParameterExpression parent)
		{
			if (action == Action.Map || this.aMapping.IsDirect || !this.aMapping.CanSynchronize)
			{
				Expression ret = this.aFrom.CreateGetterExpression(from);

				if (!this.aMapping.IsDirect)
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

				if (!this.aTo.Readable)
					// TODO: ???
					throw new InvalidMappingException($"Cannot synchronize object with setter method mapping destination without any getter method defined for {this.aTo} member of {this.aTo.DeclaringType} type");

				Expression synchronize = Expression.Call(
					Expression.Constant(this.aMapping),
					MappingMethods.Synchronize(this.aFrom.Type, this.aTo.Type),
					tempFromValue, this.aTo.CreateGetterExpression(to)
				);

				if (this.aMapping.SynchronizeCanChangeObject)
				{
					synchronize = this.aTo.CreateSetterExpression(
						to,
						synchronize
					);
				}

				if (this.aMapping.CanMap && !this.aTo.Type.IsValueType)
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

				if (this.aTo.Writable && !this.aFrom.Type.IsValueType)
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
			return $"{this.aFrom} => {this.aTo}";
		}
	}
}
