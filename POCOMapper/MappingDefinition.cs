using System;
using System.Collections.Generic;

namespace POCOMapper
{
	public class MappingDefinition
	{
		private List<SingleMappingDefinition> aMappingDefinitions;
		private bool aFinished;

		protected MappingDefinition()
		{
			this.aMappingDefinitions = new List<SingleMappingDefinition>();
			this.aFinished = false;
		}

		protected SingleMappingDefinition<TFrom, TTo> CreateMap<TFrom, TTo>()
		{
			if (aFinished)
				throw new Exception("Cannot modify the mapping");

			SingleMappingDefinition<TFrom, TTo> mappingDefinitionDef = new SingleMappingDefinition<TFrom, TTo>();
			this.aMappingDefinitions.Add(mappingDefinitionDef);
			return mappingDefinitionDef;
		}
	}
}
