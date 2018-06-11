using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Standard
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
