using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Standard
{
	public class ToStringRules<TFrom> : IMappingRules<TFrom, string>
	{
		#region Implementation of IMappingRules

		public IMapping<TFrom, string> Create(MappingImplementation mapping)
		{
			return new ToString<TFrom>(mapping);
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
