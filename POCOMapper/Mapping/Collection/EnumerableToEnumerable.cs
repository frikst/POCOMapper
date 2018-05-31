using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using KST.POCOMapper.definition;
using KST.POCOMapper.exceptions;

namespace KST.POCOMapper.mapping.collection
{
	public class EnumerableToEnumerable<TFrom, TTo> : CompiledCollectionMapping<TFrom, TTo>
	{
		private readonly bool aToIEnumerable;

		public EnumerableToEnumerable(MappingImplementation mapping, Delegate selectIdFrom, Delegate selectIdTo)
			: base(mapping, selectIdFrom, selectIdTo)
		{
			if (typeof(TTo).IsGenericType && typeof(TTo).GetGenericTypeDefinition() == typeof(IEnumerable<>))
				this.aToIEnumerable = true;
			else
				this.aToIEnumerable = false;
		}

		public override bool IsDirect
		{
			get { return false; }
		}

		protected override Expression<Func<TFrom, TTo>> CompileMapping()
		{
			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");

			if (this.aToIEnumerable)
			{
				return this.CreateMappingEnvelope(
					from,
					this.CreateItemMappingExpression(from)
				);
			}
			else
			{
				ConstructorInfo constructTo = typeof(TTo).GetConstructor(
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
					null,
					new Type[] { typeof(IEnumerable<>).MakeGenericType(this.ItemTo) },
					null
				);

				if (constructTo == null)
					throw new InvalidMapping(string.Format("Cannot find constructor for type {0}", typeof(TTo).FullName));

				return this.CreateMappingEnvelope(
					from,
					Expression.New(constructTo,
						this.CreateItemMappingExpression(from)
					)
				);
			}
		}
	}
}
