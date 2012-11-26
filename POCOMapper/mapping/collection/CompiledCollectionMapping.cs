using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using POCOMapper.definition;
using POCOMapper.exceptions;
using POCOMapper.@internal;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.collection
{
	public abstract class CompiledCollectionMapping<TFrom, TTo> : CompiledMapping<TFrom, TTo>
	{
		protected Type ItemFrom { get; private set; }
		protected Type ItemTo { get; private set; }

		protected CompiledCollectionMapping(MappingImplementation mapping)
			: base(mapping)
		{
			if (typeof(TFrom).IsArray)
				this.ItemFrom = typeof(TFrom).GetElementType();
			else if (typeof(TFrom).IsGenericType && typeof(TFrom).GetGenericTypeDefinition() == typeof(IEnumerable<>))
				this.ItemFrom = typeof(TFrom).GetGenericArguments()[0];
			else
				this.ItemFrom = typeof(TFrom).GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)).GetGenericArguments()[0];

			if (typeof(TTo).IsArray)
				this.ItemTo = typeof(TTo).GetElementType();
			else if (typeof(TTo).IsGenericType && typeof(TTo).GetGenericTypeDefinition() == typeof(IEnumerable<>))
				this.ItemTo = typeof(TTo).GetGenericArguments()[0];
			else
				this.ItemTo = typeof(TTo).GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)).GetGenericArguments()[0];
		}

		public override IEnumerable<Tuple<string, IMapping>> Children
		{
			get
			{
				yield return new Tuple<string, IMapping>("[item]", this.GetMapping());
			}
		}

		public override bool CanSynchronize
		{
			get { return false; }
		}

		public override bool CanMap
		{
			get { return true; }
		}

		protected override Expression<Action<TFrom, TTo>> CompileSynchronization()
		{
			throw new NotImplementedException();
		}

		protected IMapping GetMapping()
		{
			if (this.ItemFrom != this.ItemTo)
			{
				IMapping mapping = this.Mapping.GetMapping(this.ItemFrom, this.ItemTo);

				if (mapping == null)
					throw new UnknownMapping(this.ItemFrom, this.ItemTo);

				if (!mapping.CanMap)
					throw new InvalidMapping(string.Format("Collection items typed as {0} and {1} cannot be mapped to each other", this.ItemFrom.Name, this.ItemTo.Name));

				return mapping;
			}
			else
				return null;
		}

		protected Expression CreateItemMappingExpression(ParameterExpression from)
		{
			Expression ret = null;
			IMapping itemMapping = this.GetMapping();

			if (itemMapping != null)
			{
				Delegate mapMethod = Delegate.CreateDelegate(
					typeof(Func<,>).MakeGenericType(this.ItemFrom, this.ItemTo),
					itemMapping,
					MappingMethods.Map(this.ItemFrom, this.ItemTo)
				);

				ret = Expression.Call(null, LinqMethods.Select(this.ItemFrom, this.ItemTo),
					from,
					Expression.Constant(mapMethod)
				);
			}
			else
			{
				ret = from;
			}

			return ret;
		}

		protected Expression<Func<TFrom, TTo>> CreateMappingEnvelope(ParameterExpression from, Expression body)
		{
			Delegate postprocess = this.Mapping.GetChildPostprocessing(typeof(TTo), this.ItemTo);

			if (postprocess == null)
			{
				return Expression.Lambda<Func<TFrom, TTo>>(body, from);
			}
			else
			{
				ParameterExpression to = Expression.Parameter(typeof(TTo), "to");
				ParameterExpression item = Expression.Parameter(this.ItemTo, "item");

				return Expression.Lambda<Func<TFrom, TTo>>(
					Expression.Block(
						new ParameterExpression[] { to },

						Expression.Assign(to, body),
						ExpressionHelper.ForEach(
							item,
							to,
							ExpressionHelper.Call(postprocess, to, item)
						),
						to
					),
					from
				);
			}
		}
	}
}
