using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using POCOMapper.definition;

namespace POCOMapper.mapping.@base
{
	public abstract class CompiledMapping<TFrom, TTo> : IMapping<TFrom, TTo>
	{
		private Func<TFrom, TTo> aMappingFnc;
		private Action<TFrom, TTo> aSynchronizationFnc;
		private readonly MappingImplementation aMapping;

		protected CompiledMapping(MappingImplementation mapping)
		{
			this.aMappingFnc = null;
			this.aSynchronizationFnc = null;
			this.aMapping = mapping;
		}

		protected MappingImplementation Mapping
		{
			get { return this.aMapping; }
		}

		#region Implementation of IMapping<in TFrom,out TTo>

		public TTo Map(TFrom from)
		{
			if (object.ReferenceEquals(from, null))
				return default(TTo);

			if (this.aMappingFnc == null)
			{
				Expression<Func<TFrom, TTo>> expression = this.CompileMapping();
				this.aMappingFnc = expression.Compile();
			}

			return this.aMappingFnc(from);
		}

		public void Synchronize(TFrom from, TTo to)
		{
			if (object.ReferenceEquals(from, to))
				return;

			if (this.aSynchronizationFnc == null)
			{
				Expression<Action<TFrom, TTo>> expression = this.CompileSynchronization();
				this.aSynchronizationFnc = expression.Compile();
			}

			this.aSynchronizationFnc(from, to);
		}

		#region Implementation of IMapping

		public abstract IEnumerable<Tuple<string, IMapping>> Children { get; }

		#endregion

		#endregion

		protected abstract Expression<Func<TFrom, TTo>> CompileMapping();
		protected abstract Expression<Action<TFrom, TTo>> CompileSynchronization();
	}
}
