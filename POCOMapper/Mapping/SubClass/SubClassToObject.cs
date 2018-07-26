using System;
using System.Collections.Generic;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.SubClass.Compilers;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.SubClass
{
	public class SubClassToObject<TFrom, TTo> : IMappingWithSyncSupport<TFrom, TTo>, ISubClassMapping
	{

		private readonly IMapping<TFrom, TTo> aDefaultMapping;
		private readonly SubClassConversion[] aConversions;
		private readonly SubClassToObjectMappingCompiler<TFrom, TTo> aMappingExpression;
		private readonly SubClassToObjectSynchronizationCompiler<TFrom, TTo> aSynchronizationExpression;

		public SubClassToObject(MappingDefinitionInformation mappingDefinition, IEnumerable<(Type From, Type To)> fromTo, IMapping<TFrom, TTo> defaultMapping)
		{
			this.aDefaultMapping = defaultMapping;

			var conversions = new List<SubClassConversion>();
			foreach (var conversion in fromTo)
			{
				var fromToMapping = mappingDefinition.UnresolvedMappings.GetUnresolvedMapping(conversion.From, conversion.To);

				conversions.Add(new SubClassConversion(conversion.From, conversion.To, fromToMapping));
			}

			if (!typeof(TTo).IsAbstract)
				conversions.Add(new SubClassConversion(typeof(TFrom), typeof(TTo), this.aDefaultMapping.AsUnresolved()));

			this.aConversions = conversions.ToArray();

			this.aMappingExpression = new SubClassToObjectMappingCompiler<TFrom, TTo>(conversions);
			this.aSynchronizationExpression = new SubClassToObjectSynchronizationCompiler<TFrom, TTo>(conversions);
		}

		public void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public bool SynchronizeCanChangeObject
			=> false;

		public Type From
			=> typeof(TFrom);

		public Type To
			=> typeof(TTo);

		public IEnumerable<ISubClassConversionMapping> Conversions
			=> this.aConversions;

		#region Implementation of IMapping<TFrom,TTo>

		public TTo Map(TFrom from)
		{
			return this.aMappingExpression.Map(from);
		}

		public TTo Synchronize(TFrom from, TTo to)
		{
			return this.aSynchronizationExpression.Synchronize(from, to);
		}

		#endregion
	}
}
