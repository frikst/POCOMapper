using System;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.SpecialRules;

namespace KST.POCOMapper.Executor.TypeMappingDefinition
{
	public class UntypedTypeMappingDefinition : IExactTypeMappingDefinition
	{
		private readonly IMappingRules aRules;

		internal UntypedTypeMappingDefinition(Type from, Type to, int priority, bool visitable, IMappingRules rules)
		{
			this.From = from;
			this.To = to;
			this.Priority = priority;
			this.Visitable = visitable;
			this.aRules = rules;
		}

		public IMapping CreateMapping(MappingDefinitionInformation mappingDefinition, Type from, Type to)
		{
			if (this.From != from || this.To != to)
				throw new InvalidOperationException($"{from.Name} and {to.Name} does not match required types");

			return this.aRules.Create(from, to, mappingDefinition);
		}

		public TRules GetSpecialRules<TRules>()
			where TRules : class, ISpecialRules
		{
			return this.aRules as TRules;
		}

		public bool IsDefinedFor(MappingDefinitionInformation mappingDefinition, Type @from, Type to)
		{
			return this.From == from && this.To == to;
		}

		public int Priority { get; }

		public bool Visitable { get; }

		public Type From { get; }

		public Type To { get; }
	}
}
