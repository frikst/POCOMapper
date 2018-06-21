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
