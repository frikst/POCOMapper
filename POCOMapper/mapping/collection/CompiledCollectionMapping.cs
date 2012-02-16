using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using POCOMapper.definition;
using POCOMapper.exceptions;
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
					throw new InvalidMapping("Collection items cannot be mapped to each other");

				return mapping;
			}
			else
				return null;
		}
	}
}
