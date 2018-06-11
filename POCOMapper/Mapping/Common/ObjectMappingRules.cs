using System;
using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Common.MemberMappings;
using KST.POCOMapper.Mapping.Common.Parser;

namespace KST.POCOMapper.Mapping.Common
{
	public class ObjectMappingRules : IMappingRules
	{
		public ObjectMappingRules()
		{
			this.CfgUseImplicitMappings = true;
			this.CfgExplicitMappings = new List<IMemberMappingDefinition>();
		}

		/// <summary>
		/// Marks the mapping to use only explicit column mapping.
		/// </summary>
		public ObjectMappingRules OnlyExplicit
		{
			get
			{
				this.CfgUseImplicitMappings = false;
				return this;
			}
		}

		public ObjectMappingRules Member(string from, string to)
		{
			SimpleMemberMappingDefinition def = new SimpleMemberMappingDefinition(from, to);
			this.CfgExplicitMappings.Add(def);

			return this;
		}

		public ObjectMappingRules Member<TFromType, TToType>(string from, string to, Action<MemberMappingDefinition<TFromType, TToType>> mappingDefinition)
		{
			MemberMappingDefinition<TFromType, TToType> def = new MemberMappingDefinition<TFromType, TToType>(from, to);
			mappingDefinition(def);
			this.CfgExplicitMappings.Add(def);

			return this;
		}

		internal bool CfgUseImplicitMappings { get; private set; }
		internal List<IMemberMappingDefinition> CfgExplicitMappings { get; private set; }

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingImplementation mapping)
		{
			IEnumerable<PairedMembers> members = this.CfgExplicitMappings.Select(x => x.CreateMapping(mapping, typeof(TFrom), typeof(TTo)));
			return new ObjectToObject<TFrom, TTo>(null, mapping, members, this.CfgUseImplicitMappings);
		}

		#endregion
	}

	public class ObjectMappingRules<TFrom, TTo> : IMappingRules<TFrom, TTo>
	{
		public ObjectMappingRules()
		{
			this.UseImplicitMappings = true;
			this.ExplicitMappings = new List<IMemberMappingDefinition>();
			this.FactoryFunction = null;
		}

		public ObjectMappingRules<TFrom, TTo> Factory(Func<TFrom, TTo> factoryFunction)
		{
			this.FactoryFunction = factoryFunction;

			return this;
		}

		/// <summary>
		/// Marks the mapping to use only explicit column mapping.
		/// </summary>
		public ObjectMappingRules<TFrom, TTo> OnlyExplicit
		{
			get
			{
				this.UseImplicitMappings = false;
				return this;
			}
		}

		public ObjectMappingRules<TFrom, TTo> Member(string from, string to)
		{
			SimpleMemberMappingDefinition def = new SimpleMemberMappingDefinition(from, to);
			this.ExplicitMappings.Add(def);

			return this;
		}

		public ObjectMappingRules<TFrom, TTo> Member<TFromType, TToType>(string from, string to, Action<MemberMappingDefinition<TFromType, TToType>> mappingDefinition)
		{
			MemberMappingDefinition<TFromType, TToType> def = new MemberMappingDefinition<TFromType, TToType>(from, to);
			mappingDefinition(def);
			this.ExplicitMappings.Add(def);

			return this;
		}

		public ObjectMappingRules<TFrom, TTo> MemberFrom<TFromType>(string from, Action<MemberMappingDefinition<TFromType, TTo>> mappingDefinition)
		{
			MemberMappingDefinition<TFromType, TTo> def = new MemberMappingDefinition<TFromType, TTo>(from, null);
			mappingDefinition(def);
			this.ExplicitMappings.Add(def);

			return this;
		}

		public ObjectMappingRules<TFrom, TTo> MemberTo<TToType>(string to, Action<MemberMappingDefinition<TFrom, TToType>> mappingDefinition)
		{
			MemberMappingDefinition<TFrom, TToType> def = new MemberMappingDefinition<TFrom, TToType>(null, to);
			mappingDefinition(def);
			this.ExplicitMappings.Add(def);

			return this;
		}

		internal bool UseImplicitMappings { get; private set; }
		internal List<IMemberMappingDefinition> ExplicitMappings { get; private set; }
		internal Func<TFrom, TTo> FactoryFunction { get; private set; }

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingImplementation mapping)
		{
			IEnumerable<PairedMembers> members = this.ExplicitMappings.Select(x => x.CreateMapping(mapping, typeof(TFrom), typeof(TTo)));
			return new ObjectToObject<TFrom, TTo>(this.FactoryFunction, mapping, members, this.UseImplicitMappings);
		}

		#endregion
	}
}
