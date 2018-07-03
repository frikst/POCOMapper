using System;
using System.Collections.Generic;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Object;

namespace KST.POCOMapper.Mapping.SubClass
{
	public class SubClassMappingRules<TFrom, TTo> : IMappingRules<TFrom, TTo>
	{
		private class DefaultRules : IRulesDefinition<TFrom, TTo>
		{
			private readonly SubClassMappingRules<TFrom, TTo> aSelf;

			public DefaultRules(SubClassMappingRules<TFrom, TTo> self)
			{
				this.aSelf = self;
			}

			public TRules Rules<TRules>() where TRules : class, IMappingRules<TFrom, TTo>, new()
			{
				TRules ret = new TRules();
				this.aSelf.aDefaultRules = ret;
				return ret;
			}
		}

		private readonly List<(Type From, Type To)> aMappings;
		private IMappingRules<TFrom, TTo> aDefaultRules;

		public SubClassMappingRules()
		{
			this.aDefaultRules = new ObjectMappingRules<TFrom, TTo>();
			this.aMappings = new List<(Type From, Type To)>();
		}

		/// <summary>
		/// Adds subclass mapping.
		/// </summary>
		/// <typeparam name="TSubFrom">Source subclass.</typeparam>
		/// <typeparam name="TSubTo">Destination subclass.</typeparam>
		/// <returns>The class definition specification object.</returns>
		public SubClassMappingRules<TFrom, TTo> Map<TSubFrom, TSubTo>()
			where TSubFrom : TFrom
			where TSubTo : TTo
		{
			this.aMappings.Add((typeof(TSubFrom), typeof(TSubTo)));

			return this;
		}

		public IRulesDefinition<TFrom, TTo> Default
			=> new DefaultRules(this);

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingDefinitionInformation mappingDefinition)
		{
			return new SubClassToObject<TFrom, TTo>(mappingDefinition, this.aMappings, this.aDefaultRules.Create(mappingDefinition));
		}

		#endregion
	}
}
