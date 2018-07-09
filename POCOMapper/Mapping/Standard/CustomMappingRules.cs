using System;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Standard
{
	public class CustomMappingRules<TFrom, TTo> : IMappingRules<TFrom, TTo>
	{
		private Func<MappingDefinitionInformation, IMapping<TFrom, TTo>> aFactory;

		public CustomMappingRules()
		{
			this.aFactory = null;
		}

		public void MappingFactory(Func<MappingDefinitionInformation, IMapping<TFrom, TTo>> factory)
		{
			this.aFactory = factory;
		}

		#region Implementation of IMappingRules<TFrom,TTo>

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingDefinitionInformation mappingDefinition)
		{
			return this.aFactory(mappingDefinition);
		}

		#endregion
	}
}
