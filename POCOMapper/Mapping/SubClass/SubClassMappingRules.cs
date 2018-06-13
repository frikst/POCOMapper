using System;
using System.Collections.Generic;
using KST.POCOMapper.Definition;
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
				this.aSelf.CfgDefaultRules = ret;
				return ret;
			}
		}

		public SubClassMappingRules()
		{
			this.CfgDefaultRules = new ObjectMappingRules<TFrom, TTo>();
			this.CfgMappings = new List<(Type From, Type To)>();
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
			this.CfgMappings.Add((typeof(TSubFrom), typeof(TSubTo)));

			return this;
		}

		public IRulesDefinition<TFrom, TTo> Default
			=> new DefaultRules(this);

		protected List<(Type From, Type To)> CfgMappings { get; }
		protected IMappingRules<TFrom, TTo> CfgDefaultRules { get; private set; }

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingImplementation mapping)
		{
			return new SubClassToObject<TFrom, TTo>(mapping, this.CfgMappings, this.CfgDefaultRules.Create(mapping));
		}

		#endregion
	}
}
