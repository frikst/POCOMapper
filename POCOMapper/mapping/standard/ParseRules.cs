using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMapper.definition;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.standard
{
	public class ParseRules<TTo> : IMappingRules<string, TTo>
	{
		#region Implementation of IMappingRules

		public IMapping<string, TTo> Create(MappingImplementation mapping)
		{
			return new Parse<TTo>(mapping);
		}

		IMapping<TCreateFrom, TCreateTo> IMappingRules.Create<TCreateFrom, TCreateTo>(MappingImplementation mapping)
		{
			return (IMapping<TCreateFrom, TCreateTo>)((IMappingRules<string, TTo>)this).Create(mapping);
		}

		#endregion
	}

	public class ParseRules : IMappingRules
	{
		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingImplementation mapping)
		{
			return (IMapping<TFrom, TTo>) new Parse<TTo>(mapping);
		}

		#endregion
	}
}
