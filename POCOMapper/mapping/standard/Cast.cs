using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using POCOMapper.definition;
using POCOMapper.exceptions;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.standard
{
	public class Cast<TFrom, TTo> : CompiledMapping<TFrom, TTo>
		where TFrom : struct
		where TTo : struct
	{
		#region Overrides of CompiledMapping<TFrom,TTo>

		public Cast(MappingImplementation mapping) : base(mapping)
		{
			if ((!typeof(TFrom).IsEnum && !typeof(TFrom).IsPrimitive) || (!typeof(TTo).IsEnum && !typeof(TTo).IsPrimitive))
				throw new InvalidMapping("You can use CastMapping only on primitive or enum types");
		}

		public override IEnumerable<Tuple<string, IMapping>> Children
		{
			get { return Enumerable.Empty<Tuple<string, IMapping>>(); }
		}

		public override bool CanSynchronize
		{
			get { return false; }
		}

		public override bool CanMap
		{
			get { return true; }
		}

		protected override Expression<Func<TFrom, TTo>> CompileMapping()
		{
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");

			return Expression.Lambda<Func<TFrom, TTo>>(
				Expression.Convert(from, typeof(TTo)),
				from
			);
		}

		protected override Expression<Action<TFrom, TTo>> CompileSynchronization()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
