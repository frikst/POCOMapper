using System;
using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Object.Compilers;
using KST.POCOMapper.Mapping.Object.Parser;
using KST.POCOMapper.Members;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Object
{
	public class ObjectToObject<TFrom, TTo> : IMappingWithSyncSupport<TFrom, TTo>, IObjectMapping
	{
		private readonly ObjectToObjectMappingCompiler<TFrom, TTo> aMappingExpression;
		private readonly ObjectToObjectSynchronizationCompiler<TFrom, TTo> aSynchronizationExpression;
		private readonly IEnumerable<PairedMembers> aMemberPairs;

		public ObjectToObject(Func<TFrom, TTo> factoryFunction, MappingDefinitionInformation mappingDefinition, IEnumerable<PairedMembers> explicitPairs, bool implicitMappings)
		{
			PairedMembers[] memberPairs;

			if (implicitMappings)
			{
				var explicitPairArray = explicitPairs.ToArray();
				var explicitSymbols = new HashSet<IMember>(explicitPairArray.Select(x => x.To));

				memberPairs = explicitPairArray
					.Concat(
						new TypePairParser(mappingDefinition, typeof(TFrom), typeof(TTo))
							.Where(x => !explicitSymbols.Contains(x.To))
					)
					.ToArray();
			}
			else
			{
				memberPairs = explicitPairs.ToArray();
			}

			this.aMemberPairs = memberPairs;

			this.aMappingExpression = new ObjectToObjectMappingCompiler<TFrom, TTo>(mappingDefinition, factoryFunction, memberPairs);
			this.aSynchronizationExpression = new ObjectToObjectSynchronizationCompiler<TFrom, TTo>(mappingDefinition, memberPairs);
		}

		public void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public bool IsDirect
			=> false;

		public bool SynchronizeCanChangeObject
			=> false;

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
			return this.aSynchronizationExpression.Synchronize(from, to);
		}

		public IEnumerable<IObjectMemberMapping> Members
			=> this.aMemberPairs;
	}
}