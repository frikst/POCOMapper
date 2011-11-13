﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using POCOMapper.definition;

namespace POCOMapper.mapping.@base
{
	public abstract class CompiledMapping<TFrom, TTo> : IMapping<TFrom, TTo>
	{
		private Func<TFrom, TTo> aFnc;
		private readonly MappingImplementation aMapping;

		protected CompiledMapping(MappingImplementation mapping)
		{
			this.aFnc = null;
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

			if (this.aFnc == null)
			{
				Expression<Func<TFrom, TTo>> expression = this.Compile();
				this.aFnc = expression.Compile();
			}

			return this.aFnc(from);
		}

		#region Implementation of IMapping

		public abstract IEnumerable<Tuple<string, IMapping>> Children { get; }

		#endregion

		#endregion

		protected abstract Expression<Func<TFrom, TTo>> Compile();
	}
}
