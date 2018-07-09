using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Standard
{
	public class CopyRules<TFromTo> : IMappingRules<TFromTo, TFromTo>
	{
		#region Implementation of IMappingRules

		public IMapping<TFromTo, TFromTo> Create(MappingDefinitionInformation mappingDefinition)
		{
			return new Copy<TFromTo>();
		}

		#endregion
	}
}
