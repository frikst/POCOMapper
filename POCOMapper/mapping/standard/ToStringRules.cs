using POCOMapper.definition;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.standard
{
	public class ToStringRules<TFrom> : IMappingRules<TFrom, string>
	{
		#region Implementation of IMappingRules

		public IMapping<TFrom, string> Create(MappingImplementation mapping)
		{
			return new ToString<TFrom>(mapping);
		}

		IMapping<TCreateFrom, TCreateTo> IMappingRules.Create<TCreateFrom, TCreateTo>(MappingImplementation mapping)
		{
			return (IMapping<TCreateFrom, TCreateTo>)((IMappingRules<TFrom, string>)this).Create(mapping);
		}

		#endregion
	}

	public class ToStringRules : IMappingRules
	{
		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingImplementation mapping)
		{
			return (IMapping<TFrom, TTo>) new ToString<TFrom>(mapping);
		}

		#endregion
	}
}
