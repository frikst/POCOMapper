using System;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Collection.Compiler;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Collection
{
	public class EnumerableToList<TFrom, TTo> : IMapping<TFrom, TTo>, ICollectionMapping
	{
		private readonly ListMappingCompiler<TFrom, TTo> aMappingExpression;
		private readonly IUnresolvedMapping aItemMapping;

		public EnumerableToList(MappingDefinitionInformation mappingDefinition)
		{
			this.aItemMapping = mappingDefinition.UnresolvedMappings.GetUnresolvedMapping(EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType);

			Delegate childPostprocessing = mappingDefinition.GetChildPostprocessing(typeof(TTo), EnumerableReflection<TTo>.ItemType);

			this.aMappingExpression = new ListMappingCompiler<TFrom, TTo>(this.aItemMapping, childPostprocessing);
		}

		public void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public bool IsDirect
			=> false;

		public Type From
			=> typeof(TFrom);

		public Type To
			=> typeof(TTo);

		public TTo Map(TFrom from)
		{
			return this.aMappingExpression.Map(from);
		}

		public Type ItemFrom
			=> EnumerableReflection<TFrom>.ItemType;

		public Type ItemTo
			=> EnumerableReflection<TTo>.ItemType;

		public IMapping ItemMapping
			=> this.aItemMapping.ResolvedMapping;
	}
}
