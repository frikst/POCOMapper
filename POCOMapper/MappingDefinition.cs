using System;
using System.Collections.Generic;
using System.Reflection;

namespace POCOMapper
{
	public abstract class MappingDefinition
	{
		private static readonly Dictionary<Type, MappingImplementation> aMappings = new Dictionary<Type,MappingImplementation>();

		private readonly List<SingleMappingDefinition> aMappingDefinitions;
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

		protected static MappingImplementation GetInstance<TMappingDefinition>()
		{
			MappingImplementation ret;

			Type mapDefType = typeof(TMappingDefinition);

			if (aMappings.TryGetValue(mapDefType, out ret))
				return ret;

			ConstructorInfo ci = mapDefType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { }, null);
			ret = new MappingImplementation(((MappingDefinition) ci.Invoke(null)).aMappingDefinitions);

			aMappings[mapDefType] = ret;

			return ret;
		}
	}
}
