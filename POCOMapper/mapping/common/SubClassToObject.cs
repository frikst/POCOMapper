using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using POCOMapper.definition;
using POCOMapper.exceptions;
using POCOMapper.@internal;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.common
{
	internal interface ISubClassToObject
	{
		IEnumerable<Tuple<Type, Type, IMapping>> GetConversions();
	}

	public class SubClassToObject<TFrom, TTo> : CompiledMapping<TFrom, TTo>, ISubClassToObject
	{
		private readonly IMapping<TFrom, TTo> aDefaultMapping;
		private readonly IEnumerable<Tuple<Type, Type>> aConversions;

		public SubClassToObject(MappingImplementation mapping, IEnumerable<Tuple<Type, Type>> fromTo, IMapping<TFrom, TTo> defaultMapping) : base(mapping)
		{
			aDefaultMapping = defaultMapping;

			List<Tuple<Type, Type>> subclassMappings = new List<Tuple<Type, Type>>(fromTo);
			this.aConversions = subclassMappings.AsReadOnly();
		}

		#region Overrides of CompiledMapping<TFrom,TTo>

		public override IEnumerable<Tuple<string, IMapping>> Children
		{
			get
			{
				foreach (var mapping in this.GetConversions())
					yield return new Tuple<string, IMapping>(string.Format("{0} => {1}", mapping.Item1.Name, mapping.Item2.Name), mapping.Item3);
			}
		}

		protected override Expression<Func<TFrom, TTo>> CompileMapping()
		{
			List<Tuple<Type, Type, IMapping>> allConversions = this.GetConversions().ToList();

			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");
			ParameterExpression to = Expression.Parameter(typeof(TTo), "to");

			LabelTarget mappingEnd = Expression.Label();

			return Expression.Lambda<Func<TFrom, TTo>>(
				Expression.Block(
					new ParameterExpression[] { to },
					Expression.Block(
						allConversions.Select(x => this.MakeIfConvertStatement(x.Item1, x.Item2, x.Item3, from, to, mappingEnd))
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

		protected override Expression<Action<TFrom, TTo>> CompileSynchronization()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Implementation of ISubClassToObject

		public IEnumerable<Tuple<Type, Type, IMapping>> GetConversions()
		{
			foreach (Tuple<Type, Type> conversion in this.aConversions)
			{
				IMapping mapping = this.Mapping.GetMapping(conversion.Item1, conversion.Item2);

				if (mapping is ISubClassToObject)
				{
					foreach (Tuple<Type, Type, IMapping> innerConversion in ((ISubClassToObject)mapping).GetConversions())
						yield return innerConversion;
				}
				else
					yield return new Tuple<Type, Type, IMapping>(conversion.Item1, conversion.Item2, mapping);
			}

			if (!typeof(TTo).IsAbstract)
				yield return new Tuple<Type, Type, IMapping>(typeof(TFrom), typeof(TTo), this.aDefaultMapping);
		}

		#endregion

		private Expression MakeIfConvertStatement(Type fromType, Type toType, IMapping mapping, ParameterExpression from, ParameterExpression to, LabelTarget mappingEnd)
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
	}
}
