using System;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Executor.TypeMappingDefinition
{
	internal class ExactTypeMappingDefinition<TFrom, TTo> : IExactTypeMappingDefinition
	{
		private readonly IMappingRules<TFrom, TTo> aRules;

		public ExactTypeMappingDefinition(int priority, bool visitable, IMappingRules<TFrom, TTo> rules)
		{
			this.Priority = priority;
			this.Visitable = visitable;
			this.aRules = rules;
		}

		public IMapping CreateMapping(MappingDefinitionInformation mappingDefinition, Type from, Type to)
		{
			if (typeof(TFrom) != from || typeof(TTo) != to)
				throw new InvalidOperationException($"{from.Name} and {to.Name} does not match required types");

			return this.aRules.Create(mappingDefinition);
		}

		public bool IsDefinedFor(MappingDefinitionInformation mappingDefinition, Type @from, Type to)
		{
			return from == typeof(TFrom) && to == typeof(TTo);
		}

		public int Priority { get; }

		public bool Visitable { get; }

		public Type From
			=> typeof(TFrom);

		public Type To
			=> typeof(TTo);
	}
}