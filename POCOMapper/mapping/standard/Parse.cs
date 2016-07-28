using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using POCOMapper.definition;
using POCOMapper.exceptions;
using POCOMapper.@internal;
using POCOMapper.mapping.@base;
using POCOMapper.visitor;

namespace POCOMapper.mapping.standard
{
	public class Parse<TTo> : CompiledMapping<string, TTo>
	{
		public Parse(MappingImplementation mapping) : base(mapping)
		{
			if (!typeof(TTo).IsEnum && !typeof(TTo).IsPrimitive)
				throw new InvalidMapping(string.Format("You can use Parse only on primitive or enum types, not {0}", typeof(TTo).Name));
		}

		#region Overrides of CompiledMapping<string,TTo>

		public override void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public override bool CanSynchronize
		{
			get { return false; }
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

		protected override Expression<Func<string, TTo>> CompileMapping()
		{
			ParameterExpression from = Expression.Parameter(typeof(string), "from");

			if (typeof(TTo).IsEnum)
			{
				return Expression.Lambda<Func<string, TTo>>(
					Expression.Convert(
						Expression.Call(
							null,
							EnumMethods.Parse(typeof(TTo)),
							Expression.Constant(typeof(TTo)),
							from
						),
						typeof(TTo)
					),
					from
				);
			}
			else
			{
				return Expression.Lambda<Func<string, TTo>>(
					Expression.Call(
						null,
						PrimitiveTypeMethods.Parse(typeof(TTo)),
						from
					),
					from
				);
			}
		}

		protected override Expression<Func<string, TTo, TTo>> CompileSynchronization()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
