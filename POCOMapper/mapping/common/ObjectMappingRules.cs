using System;
using System.Collections.Generic;
using System.Linq;
using POCOMapper.definition;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.common.memberMappings;
using POCOMapper.mapping.common.parser;

namespace POCOMapper.mapping.common
{
	public class ObjectMappingRules<TFrom, TTo> : IMappingRules<TFrom, TTo>
	{
		private bool aUseImplicitMappings;
		private readonly List<IMemberMappingDefinition> aExplicitMappings;
		private Func<TFrom, TTo> aFactoryFunction;

		public ObjectMappingRules()
		{
			this.aUseImplicitMappings = true;
			this.aExplicitMappings = new List<IMemberMappingDefinition>();
			this.aFactoryFunction = null;
		}

		public ObjectMappingRules<TFrom, TTo> Factory(Func<TFrom, TTo> factoryFunction)
		{
			this.aFactoryFunction = factoryFunction;

			return this;
		}

		/// <summary>
		/// Marks the mapping to use only explicit column mapping.
		/// </summary>
		public ObjectMappingRules<TFrom, TTo> OnlyExplicit
		{
			get
			{
				this.aUseImplicitMappings = false;
				return this;
			}
		}

		public ObjectMappingRules<TFrom, TTo> Member(string from, string to)
		{
			SimpleMemberMappingDefinition def = new SimpleMemberMappingDefinition(typeof(TFrom), typeof(TTo), from, to);
			this.aExplicitMappings.Add(def);

			return this;
		}

		public ObjectMappingRules<TFrom, TTo> Member<TFromType, TToType>(string from, string to, Action<MemberMappingDefinition<TFromType, TToType>> mappingDefinition)
		{
			MemberMappingDefinition<TFromType, TToType> def = new MemberMappingDefinition<TFromType, TToType>(typeof(TFrom), typeof(TTo), from, to);
			mappingDefinition(def);
			this.aExplicitMappings.Add(def);

			return this;
		}

		public ObjectMappingRules<TFrom, TTo> MemberFrom<TFromType>(string from, Action<MemberMappingDefinition<TFromType, TTo>> mappingDefinition)
		{
			MemberMappingDefinition<TFromType, TTo> def = new MemberMappingDefinition<TFromType, TTo>(typeof(TFrom), typeof(TTo), from, null);
			mappingDefinition(def);
			this.aExplicitMappings.Add(def);

			return this;
		}

		public ObjectMappingRules<TFrom, TTo> MemberTo<TToType>(string to, Action<MemberMappingDefinition<TFrom, TToType>> mappingDefinition)
		{
			MemberMappingDefinition<TFrom, TToType> def = new MemberMappingDefinition<TFrom, TToType>(typeof(TFrom), typeof(TTo), null, to);
			mappingDefinition(def);
			this.aExplicitMappings.Add(def);

			return this;
		}

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingImplementation mapping)
		{
			IEnumerable<PairedMembers> members = this.aExplicitMappings.Select(x => x.CreateMapping(mapping));
			return new ObjectToObject<TFrom, TTo>(aFactoryFunction, mapping, members, this.aUseImplicitMappings);
		}

		IMapping<TCreateFrom, TCreateTo> IMappingRules.Create<TCreateFrom, TCreateTo>(MappingImplementation mapping)
		{
			return (IMapping<TCreateFrom, TCreateTo>)((IMappingRules<TFrom, TTo>)this).Create(mapping);
		}

		#endregion
	}
}
