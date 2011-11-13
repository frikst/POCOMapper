using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using POCOMapper.definition;
using POCOMapper.exceptions;
using POCOMapper.@internal;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.collection
{
	public class EnumerableToList<TFrom, TTo> : CompiledMapping<TFrom, TTo>
	{
		private readonly Type aItemFrom;
		private readonly Type aItemTo;

		public EnumerableToList(MappingImplementation mapping)
			: base(mapping)
		{
			this.aItemFrom = typeof(TFrom).GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)).GetGenericArguments()[0];
			this.aItemTo = typeof(TTo).GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)).GetGenericArguments()[0];
		}

		public override IEnumerable<Tuple<string, IMapping>> Children
		{
			get
			{
				yield return new Tuple<string, IMapping>("[item]", this.GetMapping());
			}
		}

		protected override Expression<Func<TFrom, TTo>> Compile()
		{
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");

			IMapping itemMapping = this.GetMapping();

			if (itemMapping != null)
			{
				Delegate mapMethod = Delegate.CreateDelegate(
					typeof(Func<,>).MakeGenericType(this.aItemFrom, this.aItemTo),
					itemMapping,
					typeof(IMapping<,>).MakeGenericType(this.aItemFrom, this.aItemTo).GetMethod("Map")
				);

				return Expression.Lambda<Func<TFrom, TTo>>(
					Expression.Call(null, LinqMethods.ToList.MakeGenericMethod(this.aItemTo),
						Expression.Call(null, LinqMethods.Select.MakeGenericMethod(this.aItemFrom, this.aItemTo),
							from,
							Expression.Constant(mapMethod)
						)
					),
					from
				);
			}
			else
			{
				return Expression.Lambda<Func<TFrom, TTo>>(
					Expression.Call(null, typeof(Enumerable).GetMethod("ToList").MakeGenericMethod(this.aItemTo),
						from
					),
					from
				);
			}
		}

		private IMapping GetMapping()
		{
			if (this.aItemFrom != this.aItemTo)
			{
				IMapping mapping = this.Mapping.GetMapping(this.aItemFrom, this.aItemTo);
				if (mapping == null)
					throw new UnknownMapping(this.aItemFrom, this.aItemTo);
				return mapping;
			}
			else
				return null;
		}
	}
}
