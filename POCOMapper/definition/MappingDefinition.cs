using System;
using System.Collections.Generic;
using System.Reflection;
using POCOMapper.conventions;
using POCOMapper.mapping.collection;

namespace POCOMapper.definition
{
	public abstract class MappingDefinition
	{
		private static readonly Dictionary<Type, MappingImplementation> aMappings = new Dictionary<Type,MappingImplementation>();

		private readonly List<IMappingDefinition> aMappingDefinitions;
		private bool aFinished;

		protected MappingDefinition()
		{
			this.aMappingDefinitions = new List<IMappingDefinition>();
			this.aFinished = false;

			this.FromConventions = new Conventions();
			this.ToConventions = new Conventions();

			this.ContainerMap<IEnumerable<T>, T[]>()
				.Using<EnumerableToArray<IEnumerable<T>, T[]>>();
			this.ContainerMap<IEnumerable<T>, List<T>>()
				.Using<EnumerableToList<IEnumerable<T>, List<T>>>();
			this.ContainerMap<IEnumerable<T>, IEnumerable<T>>()
				.Using<EnumerableToEnumerable<IEnumerable<T>, IEnumerable<T>>>();
		}

		protected Conventions FromConventions { get; private set; }
		protected Conventions ToConventions { get; private set; }

		protected ClassMappingDefinition<TFrom, TTo> Map<TFrom, TTo>()
		{
			if (aFinished)
				throw new Exception("Cannot modify the mapping");

			ClassMappingDefinition<TFrom, TTo> mappingDefinitionDef = new ClassMappingDefinition<TFrom, TTo>();
			this.aMappingDefinitions.Add(mappingDefinitionDef);
			return mappingDefinitionDef;
		}

		protected ContainerMappingDefinition<TFrom, TTo> ContainerMap<TFrom, TTo>()
			where TFrom : IEnumerable<T>
			where TTo : IEnumerable<T>
		{
			if (aFinished)
				throw new Exception("Cannot modify the mapping");

			ContainerMappingDefinition<TFrom, TTo> mappingDefinitionDef = new ContainerMappingDefinition<TFrom, TTo>();
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
