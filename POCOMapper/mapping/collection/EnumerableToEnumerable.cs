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
	public class EnumerableToEnumerable<TFrom, TTo> : CompiledMapping<TFrom, TTo>
		where TFrom : class
		where TTo : class
	{
		private readonly Type aItemFrom;
		private readonly Type aItemTo;

		public EnumerableToEnumerable(MappingImplementation mapping)
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

		protected override Expression<Func<TFrom, TTo>> CompileMapping()
		{
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");

			ConstructorInfo constructTo = typeof(TTo).GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(this.aItemTo) });

			IMapping itemMapping = this.GetMapping();

			if (itemMapping != null)
			{
				Delegate mapMethod = Delegate.CreateDelegate(
					typeof(Func<,>).MakeGenericType(this.aItemFrom, this.aItemTo),
					itemMapping,
					MappingMethods.Map(this.aItemFrom, this.aItemTo)
				);

				return Expression.Lambda<Func<TFrom, TTo>>(
					Expression.New(constructTo,
						Expression.Call(null, LinqMethods.Select(this.aItemFrom, this.aItemTo),
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
					Expression.New(constructTo,
						from
					),
					from
				);
			}
		}

		protected override Expression<Action<TFrom, TTo>> CompileSynchronization()
		{
			throw new NotImplementedException();
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
