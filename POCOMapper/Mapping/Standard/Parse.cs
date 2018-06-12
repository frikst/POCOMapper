using System;
using System.Linq.Expressions;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Standard
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
			=> false;

		public override bool CanMap
			=> true;

		public override bool IsDirect
			=> false;

		public override bool SynchronizeCanChangeObject
			=> false;

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
