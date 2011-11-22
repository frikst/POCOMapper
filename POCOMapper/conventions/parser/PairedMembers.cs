using System.Linq.Expressions;
using POCOMapper.exceptions;
using POCOMapper.@internal;
using POCOMapper.mapping.@base;

namespace POCOMapper.conventions.parser
{
	public class PairedMembers
	{
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

		public Expression CreateAssignmentExpression(ParameterExpression from, ParameterExpression to, bool syncOnly)
		{
			if (!syncOnly || this.aTo.Type.IsValueType || this.aTo.Type.IsAssignableFrom(typeof(string)))
			{
				Expression ret = this.aFrom.CreateGetterExpression(from);

				if (this.aMapping != null)
					ret = Expression.Call(
						Expression.Constant(this.aMapping),
						MappingMethods.Map(this.aFrom.Type, this.aTo.Type),
						ret
					);

				return this.aTo.CreateSetterExpression(
					to,
					ret
				);
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

				return Expression.Block(
					new ParameterExpression[] { tempFromValue, tempToValue },
					Expression.Assign(tempFromValue, this.aFrom.CreateGetterExpression(from)),
					Expression.Assign(tempToValue, this.aTo.CreateGetterExpression(to)),
					Expression.IfThenElse(
						Expression.Equal(tempFromValue, Expression.Constant(null)),
						this.aTo.CreateSetterExpression(to, Expression.Constant(null)),
						Expression.IfThenElse(
							Expression.Equal(tempToValue, Expression.Constant(null)),
							this.aTo.CreateSetterExpression(
								to,
								Expression.Call(
									Expression.Constant(this.aMapping),
									MappingMethods.Map(this.aFrom.Type, this.aTo.Type),
									tempFromValue
								)
							),
							Expression.Call(
								Expression.Constant(this.aMapping),
								MappingMethods.Synchronize(this.aFrom.Type, this.aTo.Type),
								tempFromValue, this.aTo.CreateGetterExpression(to)
							)
						)
					)
				);
			}
		}
	}
}
