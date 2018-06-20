using System;
using System.Linq.Expressions;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Collection.Compiler;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Collection
{
	public class EnumerableToList<TFrom, TTo> : IMapping<TFrom, TTo>, ICollectionMapping
	{
		private readonly ListMappingCompiler<TFrom, TTo> aMappingExpression;
		private readonly IUnresolvedMapping aItemMapping;

		public EnumerableToList(MappingImplementation mapping)
		{
			this.aItemMapping = mapping.GetUnresolvedMapping(EnumerableReflection<TFrom>.ItemType, EnumerableReflection<TTo>.ItemType);

			Delegate childPostprocessing = mapping.GetChildPostprocessing(typeof(TTo), EnumerableReflection<TTo>.ItemType);

			this.aMappingExpression = new ListMappingCompiler<TFrom, TTo>(this.aItemMapping, childPostprocessing);
		}

		public void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public bool CanSynchronize
			=> false;

		public bool CanMap
			=> true;

		public bool IsDirect
			=> false;

		public bool SynchronizeCanChangeObject
			=> true;

		public string MappingSource
			=> this.aMappingExpression.Source;

		public string SynchronizationSource
			=> null;

		public Type From
			=> typeof(TFrom);

		public Type To
			=> typeof(TTo);

		public TTo Map(TFrom from)
		{
			return this.aMappingExpression.Map(from);
		}

		public TTo Synchronize(TFrom from, TTo to)
		{
			throw new NotImplementedException();
		}

		public Type ItemFrom
			=> EnumerableReflection<TFrom>.ItemType;

		public Type ItemTo
			=> EnumerableReflection<TTo>.ItemType;

		public IMapping ItemMapping
			=> this.aItemMapping.ResolvedMapping;
	}
}
