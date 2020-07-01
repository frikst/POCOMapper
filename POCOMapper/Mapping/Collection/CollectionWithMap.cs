using System;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Collection.Compiler;
using KST.POCOMapper.Mapping.MappingCompilaton;
using KST.POCOMapper.SpecialRules;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Collection
{
	public class CollectionWithMap<TFrom, TTo> : IMapping<TFrom, TTo>, IMappingWithSpecialComparision<TFrom, TTo>, ICollectionMapping
	{
		private readonly CollectionMappingCompiler<TFrom, TTo> aMappingExpression;

		private readonly IUnresolvedMapping aItemMapping;
        private readonly EnumerableComparisionCompiler<TFrom, TTo> aMapEqualityExpression;

        internal CollectionWithMap(MappingDefinitionInformation mappingDefinition, IEqualityRules equalityRules, bool mapNullToEmpty)
		{
			this.aItemMapping = mappingDefinition.UnresolvedMappings.GetUnresolvedMapping(EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType);

			var childPostprocessing = mappingDefinition.GetChildPostprocessing(typeof(TTo), EnumerableReflection<TTo>.ItemType);

			if (ArrayMappingCompiler<TFrom, TTo>.ShouldUse())
				this.aMappingExpression = new ArrayMappingCompiler<TFrom, TTo>(this.aItemMapping, childPostprocessing, mapNullToEmpty);
			else if (ListMappingCompiler<TFrom, TTo>.ShouldUse())
				this.aMappingExpression = new ListMappingCompiler<TFrom, TTo>(this.aItemMapping, childPostprocessing, mapNullToEmpty);
			else if (ConstructorMappingCompiler<TFrom, TTo>.ShouldUse())
				this.aMappingExpression = new ConstructorMappingCompiler<TFrom, TTo>(this.aItemMapping, childPostprocessing, mapNullToEmpty);
			else
				throw new InvalidMappingException($"Cannot find proper method to map to a collection of type {typeof(TTo).FullName}");

            this.aMapEqualityExpression = new EnumerableComparisionCompiler<TFrom, TTo>(this.aItemMapping, equalityRules, mapNullToEmpty);
		}

        public void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public Type From
			=> typeof(TFrom);

		public Type To
			=> typeof(TTo);

		public TTo Map(TFrom from)
		{
			return this.aMappingExpression.Map(from);
		}

        public bool MapEqual(TFrom from, TTo to)
        {
            return this.aMapEqualityExpression.MapEqual(from, to);
        }

		public Type ItemFrom
			=> EnumerableReflection<TFrom>.ItemType;

		public Type ItemTo
			=> EnumerableReflection<TTo>.ItemType;

		public IMapping ItemMapping
			=> this.aItemMapping.ResolvedMapping;
	}
}
