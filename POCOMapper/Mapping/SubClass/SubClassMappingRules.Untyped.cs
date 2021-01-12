using System;
using System.Collections.Generic;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Object;

namespace KST.POCOMapper.Mapping.SubClass
{
	public class SubClassMappingRules : IMappingRules
	{
		private class DefaultRules : IRulesDefinition
		{
			private readonly SubClassMappingRules aSelf;

			public DefaultRules(SubClassMappingRules self)
			{
				this.aSelf = self;
			}

			public TRules Rules<TRules>() where TRules : class, IMappingRules, new()
			{
				TRules ret = new TRules();
				this.aSelf.aDefaultRules = ret;
				return ret;
			}
		}

		private readonly List<(Type From, Type To)> aMappings;
		private IMappingRules aDefaultRules;

		public SubClassMappingRules()
		{
			this.aDefaultRules = new ObjectMappingRules();
			this.aMappings = new List<(Type From, Type To)>();
		}

		/// <summary>
		/// Adds subclass mapping.
		/// </summary>
		/// <param name="subFrom">Source subclass.</param>
		/// <param name="subTo">Destination subclass.</param>
		/// <returns>The class definition specification object.</returns>
		public SubClassMappingRules Map(Type subFrom, Type subTo)
		{
			this.aMappings.Add((subFrom, subTo));

			return this;
		}

		public IRulesDefinition Default
			=> new DefaultRules(this);

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingDefinitionInformation mappingDefinition)
		{
			return new SubClassToObject<TFrom, TTo>(mappingDefinition, this.aMappings, this.aDefaultRules.Create<TFrom, TTo>(mappingDefinition));
		}

		#endregion
	}
}
