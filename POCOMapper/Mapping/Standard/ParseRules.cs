using KST.POCOMapper.definition;
using KST.POCOMapper.mapping.@base;

namespace KST.POCOMapper.mapping.standard
{
	public class ParseRules<TTo> : IMappingRules<string, TTo>
	{
		#region Implementation of IMappingRules

		public IMapping<string, TTo> Create(MappingImplementation mapping)
		{
			return new Parse<TTo>(mapping);
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
