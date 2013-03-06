using POCOMapper.definition;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.standard
{
	public class CastRules<TFrom, TTo> : IMappingRules<TFrom, TTo>
		where TFrom : struct
		where TTo : struct
	{
		#region Implementation of IMappingRules

		public IMapping<TFrom, TTo> Create(MappingImplementation mapping)
		{
			return new Cast<TFrom, TTo>(mapping);
		}

		IMapping<TCreateFrom, TCreateTo> IMappingRules.Create<TCreateFrom, TCreateTo>(MappingImplementation mapping)
		{
			return (IMapping<TCreateFrom, TCreateTo>)((IMappingRules<TFrom, TTo>)this).Create(mapping);
		}

		#endregion
	}

	public class CastRules : IMappingRules
	{
		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingImplementation mapping)
		{
			return new Cast<TFrom, TTo>(mapping);
		}

		#endregion
	}
}
