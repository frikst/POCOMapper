using System;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Standard
{
	public class CustomMappingRules : IMappingRules
	{
		private Func<MappingDefinitionInformation, Type, Type, IMapping> aFactory;

		public CustomMappingRules()
		{
			this.aFactory = null;
		}

		public void MappingFactory(Func<MappingDefinitionInformation, Type, Type, IMapping> factory)
		{
			this.aFactory = factory;
		}

		#region Implementation of IMappingRules

		public IMapping<TFrom, TTo> Create<TFrom, TTo>(MappingDefinitionInformation mappingDefinition)
		{
			return (IMapping<TFrom, TTo>) this.aFactory(mappingDefinition, typeof(TFrom), typeof(TTo));
		}

		#endregion
	}
}
