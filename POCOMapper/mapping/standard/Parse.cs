using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using POCOMapper.definition;
using POCOMapper.exceptions;
using POCOMapper.@internal;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.standard
{
	public class Parse<TTo> : CompiledMapping<string, TTo>
	{
		public Parse(MappingImplementation mapping) : base(mapping)
		{
			if (!typeof(TTo).IsEnum && !typeof(TTo).IsPrimitive)
				throw new InvalidMapping("You can use CastMapping only on primitive or enum types");
		}

		#region Overrides of CompiledMapping<string,TTo>

		public override IEnumerable<Tuple<string, IMapping>> Children
		{
			get { return Enumerable.Empty<Tuple<string, IMapping>>(); }
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

		protected override Expression<Action<string, TTo>> CompileSynchronization()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
