using System;
using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Executor.TypeMappingDefinition;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Executor
{
	internal class TypeMappingDefinitionContainer
	{
		private readonly MappingDefinitionInformation aMappingDefinition;
		private readonly List<ITypeMappingDefinition> aTypeMappingDefinitions;

		public TypeMappingDefinitionContainer(MappingDefinitionInformation mappingDefinition, IEnumerable<ITypeMappingDefinition> mappingDefinitions)
		{
			this.aMappingDefinition = mappingDefinition;

			this.aTypeMappingDefinitions = mappingDefinitions.OrderBy(x => x.Priority).ToList();
		}

		public IEnumerable<TypePair> VisitableMappings
			=> this.aTypeMappingDefinitions.OfType<IExactTypeMappingDefinition>().Where(x => x.Visitable).Select(x => new TypePair(x.From, x.To));

		public bool TryCreateMapping(Type from, Type to, out IMapping mapping)
		{
			foreach (var currentDefinition in this.aTypeMappingDefinitions)
			{
				if (currentDefinition.IsDefinedFor(this.aMappingDefinition, @from, to))
				{
					mapping = currentDefinition.CreateMapping(this.aMappingDefinition, from, to);
					return true;
				}
			}

			mapping = null;
			return false;
		}

		public bool ContainsMapping(Type from, Type to)
		{
			return this.aTypeMappingDefinitions.Any(x => x.IsDefinedFor(this.aMappingDefinition, from, to));
		}
	}
}
