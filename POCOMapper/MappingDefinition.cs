using System;
using System.Collections.Generic;
using System.Reflection;
using POCOMapper.conventions;

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

			this.FromConventions = new Conventions();
			this.ToConventions = new Conventions();
		}

		protected Conventions FromConventions { get; private set; }
		protected Conventions ToConventions { get; private set; }

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
			MappingDefinition definition = (MappingDefinition) ci.Invoke(null);
			definition.aFinished = true;

			ret = new MappingImplementation(definition.aMappingDefinitions, definition.FromConventions, definition.ToConventions);

			aMappings[mapDefType] = ret;

			return ret;
		}
	}
}
