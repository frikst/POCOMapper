﻿using System;
using System.Linq.Expressions;
using KST.POCOMapper.conventions.members;
using KST.POCOMapper.exceptions;
using KST.POCOMapper.@internal;
using KST.POCOMapper.mapping.@base;

namespace KST.POCOMapper.mapping.common.parser
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
				throw new Exception(string.Format("Cannot map {0} and {1} together", from, to));

			this.aFrom = from;
			this.aTo = to;

			if (mapping == null || mapping.IsDirect)
				this.aMapping = null;
			else
				this.aMapping = mapping;
		}

		public IMember From
		{
			get { return this.aFrom; }
		}

		public IMember To
		{
			get { return this.aTo; }
		}

		public IMapping Mapping
		{
			get { return this.aMapping; }
		}

		public Expression CreateAssignmentExpression(ParameterExpression from, ParameterExpression to, Action action, Delegate postprocess, ParameterExpression parent)
		{
			if (
				action == Action.Map
				|| this.aMapping == null
				|| (this.aMapping != null && !this.aMapping.CanSynchronize)
			)
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
					throw new InvalidMapping(string.Format("Cannot synchronize object with setter method mapping destination without any getter method defined for {0} member of {1} type", this.aTo, this.aTo.DeclaringType));

				if (this.aMapping == null)
					// TODO: ???
					throw new InvalidMapping(string.Format("Cannot synchronize two reference objects with the same type for {0} member of {1} type", this.aTo, this.aTo.DeclaringType));

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

				if (this.aTo.Setter != null && !this.aFrom.Type.IsValueType)
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