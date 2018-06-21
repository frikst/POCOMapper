using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Standard
{
	public class CastRules<TFrom, TTo> : IMappingRules<TFrom, TTo>
		where TFrom : struct
		where TTo : struct
	{
		#region Implementation of IMappingRules

		public IMapping<TFrom, TTo> Create(MappingDefinitionInformation mappingDefinition)
		{
			return new Cast<TFrom, TTo>();
		}

		#endregion
	}

	public class CastRules : IMappingRules
	{
		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingDefinitionInformation mappingDefinition)
		{
			return new Cast<TFrom, TTo>();
		}

		#endregion
	}
}
