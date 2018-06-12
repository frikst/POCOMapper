using System;
using System.Linq.Expressions;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Standard
{
	public class Cast<TFrom, TTo> : CompiledMapping<TFrom, TTo>
	{
		#region Overrides of CompiledMapping<TFrom,TTo>

		public Cast(MappingImplementation mapping) : base(mapping)
		{
			if ((!typeof(TFrom).IsEnum && !typeof(TFrom).IsPrimitive) || (!typeof(TTo).IsEnum && !typeof(TTo).IsPrimitive))
				throw new InvalidMapping(string.Format("You can use CastMapping only on primitive or enum types, not {0} and {1}", typeof(TFrom).Name, typeof(TTo).Name));
		}

		public override void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public override bool CanSynchronize
			=> false;

		public override bool CanMap
			=> true;

		public override bool IsDirect
			=> false;

		public override bool SynchronizeCanChangeObject
			=> false;

		protected override Expression<Func<TFrom, TTo>> CompileMapping()
		{
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");

			return Expression.Lambda<Func<TFrom, TTo>>(
				Expression.Convert(from, typeof(TTo)),
				from
			);
		}

		protected override Expression<Func<TFrom, TTo, TTo>> CompileSynchronization()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
