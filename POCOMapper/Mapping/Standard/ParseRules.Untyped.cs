using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Standard
{
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
