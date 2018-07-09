using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Standard
{
	public class ToStringRules<TFrom> : IMappingRules<TFrom, string>
	{
		#region Implementation of IMappingRules

		public IMapping<TFrom, string> Create(MappingDefinitionInformation mappingDefinition)
		{
			return new ToString<TFrom>();
		}

		#endregion
	}
}
