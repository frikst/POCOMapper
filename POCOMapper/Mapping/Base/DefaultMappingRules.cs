using POCOMapper.definition;

namespace POCOMapper.mapping.@base
{
	/// <summary>
	/// For internal use only. Use with caution: Can cause infinite recursion.
	/// </summary>
	internal class DefaultMappingRules<TFrom, TTo> : IMappingRules<TFrom, TTo>
	{
		#region Implementation of IMappingRules

		public IMapping<TFrom, TTo> Create(MappingImplementation mapping)
		{
			return mapping.GetMapping<TFrom, TTo>();
		}

		#endregion
	}

	/// <summary>
	/// For internal use only. Use with caution: Can cause infinite recursion.
	/// </summary>
	internal class DefaultMappingRules : IMappingRules
	{
		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingImplementation mapping)
		{
			return mapping.GetMapping<TFrom, TTo>();
		}

		#endregion
	}
}
