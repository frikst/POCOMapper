using System;
using System.Linq.Expressions;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Members;

namespace KST.POCOMapper.Mapping.Object.Parser
{
	public class PairedMembers : IObjectMemberMapping
	{
		public PairedMembers(IMember from, IMember to, IUnresolvedMapping mapping)
		{
			if (!from.CanPairWith(to))
				throw new Exception($"Cannot map {from} and {to} together");

			this.From = from;
			this.To = to;

			this.Mapping = mapping;
		}

		public IMember From { get; }

		public IMember To { get; }

		public IUnresolvedMapping Mapping { get; }

		IMapping IObjectMemberMapping.Mapping
			=> this.Mapping.ResolvedMapping;

		public Expression CreateMappingAssignmentExpression(ParameterExpression from, ParameterExpression to, Delegate postprocess, ParameterExpression parent)
		{
			Expression ret = this.From.CreateGetterExpression(from);

			if (!(this.Mapping.ResolvedMapping is IDirectMapping))
			{
				ret = Expression.Call(
					Expression.Constant(this.Mapping.ResolvedMapping),
					MappingMethods.Map(this.From.Type, this.To.Type),
					ret
				);
			}

			ret = this.To.CreateSetterExpression(
				to,
				ret
			);

			return this.AddPostprocess(ret, to, postprocess, parent);
		}

		public Expression CreateSynchronizationAssignmentExpression(ParameterExpression from, ParameterExpression to, Delegate postprocess, ParameterExpression parent)
		{
			IMappingWithSyncSupport mappingWithSync = this.Mapping.ResolvedMapping as IMappingWithSyncSupport;

			if (mappingWithSync == null)
				return this.CreateMappingAssignmentExpression(from, to, postprocess, parent);

			ParameterExpression tempFromValue = Expression.Parameter(this.From.Type, "tempFrom");
			ParameterExpression tempToValue = Expression.Parameter(this.To.Type, "tempTo");

			if (!this.To.Readable)
				// TODO: ???
				throw new InvalidMappingException($"Cannot synchronize object with setter method mapping destination without any getter method defined for {this.To} member of {this.To.DeclaringType} type");

			Expression synchronize = Expression.Call(
				Expression.Constant(mappingWithSync),
				MappingMethods.Synchronize(this.From.Type, this.To.Type),
				tempFromValue, tempToValue
			);

			if (mappingWithSync.SynchronizeCanChangeObject)
			{
				if (!this.To.Writable)
					throw new InvalidMappingException($"Cannot synchronize non writtable member {this.To} using mapping {mappingWithSync.GetType().Name}");

				if (postprocess != null)
				{
					var origToValue = Expression.Parameter(this.To.Type, "origTo");
					var newToValue = Expression.Parameter(this.To.Type, "newTo");

					synchronize = Expression.Block(
						new[] {origToValue, newToValue},
						Expression.Assign(origToValue, tempToValue),
						Expression.Assign(newToValue, synchronize),
						Expression.IfThen(
							ExpressionHelper.NotSame(origToValue, newToValue),
							ExpressionHelper.Call(postprocess, parent, newToValue)
						),
						this.To.CreateSetterExpression(
							to,
							newToValue
						)
					);
				}
				else
				{
					synchronize = this.To.CreateSetterExpression(
						to,
						synchronize
					);
				}
			}

			return Expression.Block(
				new ParameterExpression[] { tempFromValue, tempToValue },
				Expression.Assign(tempFromValue, this.From.CreateGetterExpression(from)),
				Expression.Assign(tempToValue, this.To.CreateGetterExpression(to)),
				synchronize
			);
		}
        
        public Expression CreateComparisionExpression(ParameterExpression from, ParameterExpression to, LabelTarget end)
        {
            if (this.Mapping.ResolvedMapping is IMappingWithSpecialComparision)
            {
                return Expression.IfThen(
                    Expression.Not( 
                        Expression.Call(
                            Expression.Constant(this.Mapping.ResolvedMapping),
                            MappingMethods.MapEqual(this.From.Type, this.To.Type),
                            this.From.CreateGetterExpression(from),
                            this.To.CreateGetterExpression(to)
                        )
                    ),
                    Expression.Return(end, Expression.Constant(false))
                );
            }
            else
            {
                var fromMapped = this.From.CreateGetterExpression(from);

                if (!(this.Mapping.ResolvedMapping is IDirectMapping))
                {
                    fromMapped = Expression.Call(
                        Expression.Constant(this.Mapping.ResolvedMapping),
                        MappingMethods.Map(this.From.Type, this.To.Type),
                        fromMapped
                    );
                }

                return Expression.IfThen(
                    Expression.Not( 
                        Expression.Call(
                            Expression.Constant(EqualityComparerMethods.GetEqualityComparer(this.To.Type)),
                            EqualityComparerMethods.Equals(this.To.Type),
                            fromMapped,
                            this.To.CreateGetterExpression(to)
                        )
                    ),
                    Expression.Return(end, Expression.Constant(false))
                );
            }
        }

		private Expression AddPostprocess(Expression assignment, ParameterExpression to, Delegate postprocess, ParameterExpression parent)
		{
			if (postprocess != null)
			{
				return Expression.Block(
					assignment,
					ExpressionHelper.Call(postprocess, parent, this.To.CreateGetterExpression(to))
				);
			}
			else
			{
				return assignment;
			}
		}

		public override string ToString()
		{
			return $"{this.From} => {this.To}";
		}
    }
}
