using KST.POCOMapper.definition;
using KST.POCOMapper.mapping.@base;

namespace KST.POCOMapper.mapping.standard
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
