using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.SubClass
{
	public class SubClassToObject<TFrom, TTo> : CompiledMapping<TFrom, TTo>, ISubClassMapping
	{
		private class SubClassConversion : ISubClassConversionMapping
		{
			public SubClassConversion(Type from, Type to, IMapping mapping)
			{
				this.From = from;
				this.To = to;
				this.Mapping = mapping;
			}

			#region Implementation of ISubClassConversionMapping

			public Type From { get; }
			public Type To { get; }
			public IMapping Mapping { get; }

			#endregion
		}

		private readonly IMapping<TFrom, TTo> aDefaultMapping;
		private readonly ReadOnlyCollection<(Type From, Type To)> aConversions;

		public SubClassToObject(MappingImplementation mapping, IEnumerable<(Type From, Type To)> fromTo, IMapping<TFrom, TTo> defaultMapping) : base(mapping)
		{
			this.aDefaultMapping = defaultMapping;

			this.aConversions = fromTo.ToList().AsReadOnly();
		}

		#region Overrides of CompiledMapping<TFrom,TTo>

		public override void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public override bool CanSynchronize
			=> true;

		public override bool CanMap
			=> true;

		public override bool IsDirect
			=> false;

		public override bool SynchronizeCanChangeObject
			=> false;

		protected override Expression<Func<TFrom, TTo>> CompileMapping()
		{
			List<ISubClassConversionMapping> allConversions = this.Conversions.ToList();

			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");
			ParameterExpression to = Expression.Parameter(typeof(TTo), "to");

			LabelTarget mappingEnd = Expression.Label();

			return Expression.Lambda<Func<TFrom, TTo>>(
				Expression.Block(
					new ParameterExpression[] { to },
					Expression.Block(
						allConversions.Select(x => this.MakeIfConvertMapStatement(x.From, x.To, x.Mapping, from, to, mappingEnd))
					),
					Expression.Throw(
						Expression.New(
							typeof(UnknownMappingException).GetConstructor(new Type[] { typeof(Type), typeof(Type) }),
							Expression.Call(from, ObjectMethods.GetType()),
							Expression.Constant(typeof(TTo))
						)
					),
					Expression.Label(mappingEnd),
					to
				),
				from
			);
		}

		protected override Expression<Func<TFrom, TTo, TTo>> CompileSynchronization()
		{
			List<ISubClassConversionMapping> allConversions = this.Conversions.ToList();

			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");
			ParameterExpression to = Expression.Parameter(typeof(TTo), "to");

			LabelTarget mappingEnd = Expression.Label();

			return Expression.Lambda<Func<TFrom, TTo, TTo>>(
				Expression.Block(
					Expression.Block(
						allConversions.Select(x => this.MakeIfConvertSynchronizeStatement(x.From, x.To, x.Mapping, from, to, mappingEnd))
					),
					Expression.Throw(
						Expression.New(
							typeof(UnknownMappingException).GetConstructor(new Type[] { typeof(Type), typeof(Type) }),
							Expression.Call(from, ObjectMethods.GetType()),
							Expression.Constant(typeof(TTo))
						)
					),
					Expression.Label(mappingEnd),
					to
				),
				from, to
			);
		}

		#endregion

		#region Implementation of ISubClassMapping

		public IEnumerable<ISubClassConversionMapping> Conversions
		{
			get
			{
				foreach (var conversion in this.aConversions)
				{
					IMapping mapping = this.Mapping.GetMapping(conversion.From, conversion.To);

					if (mapping is ISubClassMapping subClassMapping)
					{
						foreach (var innerConversion in subClassMapping.Conversions)
							yield return innerConversion;
					}
					else
						yield return new SubClassConversion(conversion.From, conversion.To, mapping);
				}

				if (!typeof(TTo).IsAbstract)
					yield return new SubClassConversion(typeof(TFrom), typeof(TTo), this.aDefaultMapping);
			}
		}

		#endregion

		private Expression MakeIfConvertMapStatement(Type fromType, Type toType, IMapping mapping, ParameterExpression from, ParameterExpression to, LabelTarget mappingEnd)
		{
			return Expression.IfThen(
				Expression.Equal(
					Expression.Call(from, ObjectMethods.GetType()),
					Expression.Constant(fromType)
				),
				Expression.Block(
					Expression.Assign(
						to,
						Expression.Call(
							Expression.Constant(mapping),
							MappingMethods.Map(fromType, toType),
							Expression.Convert(from, fromType)
						)
					),
					Expression.Goto(mappingEnd)
				)
			);
		}

		private Expression MakeIfConvertSynchronizeStatement(Type fromType, Type toType, IMapping mapping, ParameterExpression from, ParameterExpression to, LabelTarget mappingEnd)
		{
			return Expression.IfThen(
				Expression.Equal(
					Expression.Call(from, ObjectMethods.GetType()),
					Expression.Constant(fromType)
				),
				Expression.Block(
					Expression.Call(
						Expression.Constant(mapping),
						MappingMethods.Synchronize(fromType, toType),
						Expression.Convert(from, fromType),
						Expression.Convert(to, toType)
					),
					Expression.Goto(mappingEnd)
				)
			);
		}
	}
}
