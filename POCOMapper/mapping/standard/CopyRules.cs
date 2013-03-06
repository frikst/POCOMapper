using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMapper.definition;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.standard
{
	public class CopyRules<TFromTo> : IMappingRules<TFromTo, TFromTo>
	{
		#region Implementation of IMappingRules

		public IMapping<TFromTo, TFromTo> Create(MappingImplementation mapping)
		{
			return new Copy<TFromTo>(mapping);
		}

		#endregion
	}

	public class CopyRules : IMappingRules
	{
		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingImplementation mapping)
		{
			return (IMapping<TFrom, TTo>) new Copy<TFrom>(mapping);
		}

		#endregion
	}
}
