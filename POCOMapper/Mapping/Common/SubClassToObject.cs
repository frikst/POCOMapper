using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KST.POCOMapper.definition;
using KST.POCOMapper.exceptions;
using KST.POCOMapper.@internal;
using KST.POCOMapper.mapping.@base;
using KST.POCOMapper.visitor;

namespace KST.POCOMapper.mapping.common
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

			public Type From { get; private set; }
			public Type To { get; private set; }
			public IMapping Mapping { get; private set; }

			#endregion
		}

		private readonly IMapping<TFrom, TTo> aDefaultMapping;
		private readonly IEnumerable<Tuple<Type, Type>> aConversions;

		public SubClassToObject(MappingImplementation mapping, IEnumerable<Tuple<Type, Type>> fromTo, IMapping<TFrom, TTo> defaultMapping) : base(mapping)
		{
			this.aDefaultMapping = defaultMapping;

			List<Tuple<Type, Type>> subclassMappings = new List<Tuple<Type, Type>>(fromTo);
			this.aConversions = subclassMappings.AsReadOnly();
		}

		#region Overrides of CompiledMapping<TFrom,TTo>

		public override void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public override bool CanSynchronize
		{
			get { return true; }
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
							typeof(UnknownMapping).GetConstructor(new Type[] { typeof(Type), typeof(Type) }),
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
							typeof(UnknownMapping).GetConstructor(new Type[] { typeof(Type), typeof(Type) }),
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
				foreach (Tuple<Type, Type> conversion in this.aConversions)
				{
					IMapping mapping = this.Mapping.GetMapping(conversion.Item1, conversion.Item2);

					if (mapping is ISubClassMapping)
					{
						foreach (var innerConversion in ((ISubClassMapping)mapping).Conversions)
							yield return innerConversion;
					}
					else
						yield return new SubClassConversion(conversion.Item1, conversion.Item2, mapping);
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
