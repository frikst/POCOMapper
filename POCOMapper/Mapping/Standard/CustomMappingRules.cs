using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMapper.definition;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.standard
{
	public class CustomMappingRules<TFrom, TTo> : IMappingRules<TFrom, TTo>
	{
		private Func<MappingImplementation, IMapping<TFrom, TTo>> aFactory;

		public CustomMappingRules()
		{
			this.aFactory = null;
		}

		public void MappingFactory(Func<MappingImplementation, IMapping<TFrom, TTo>> factory)
		{
			this.aFactory = factory;
		}

		#region Implementation of IMappingRules<TFrom,TTo>

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingImplementation mapping)
		{
			return this.aFactory(mapping);
		}

		#endregion
	}

	public class CustomMappingRules : IMappingRules
	{
		private Func<MappingImplementation, Type, Type, IMapping> aFactory;

		public CustomMappingRules()
		{
			this.aFactory = null;
		}

		public void MappingFactory(Func<MappingImplementation, Type, Type, IMapping> factory)
		{
			this.aFactory = factory;
		}

		#region Implementation of IMappingRules

		public IMapping<TFrom, TTo> Create<TFrom, TTo>(MappingImplementation mapping)
		{
			return (IMapping<TFrom, TTo>) this.aFactory(mapping, typeof(TFrom), typeof(TTo));
		}

		#endregion
	}
}
