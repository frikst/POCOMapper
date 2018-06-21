using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Standard
{
	public class ParseRules<TTo> : IMappingRules<string, TTo>
	{
		#region Implementation of IMappingRules

		public IMapping<string, TTo> Create(MappingDefinitionInformation mappingDefinition)
		{
			return new Parse<TTo>();
		}

		#endregion
	}

	public class ParseRules : IMappingRules
	{
		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingDefinitionInformation mappingDefinition)
		{
			return (IMapping<TFrom, TTo>) new Parse<TTo>();
		}

		#endregion
	}
}
