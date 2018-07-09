using System;
using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Object.MemberMappings;

namespace KST.POCOMapper.Mapping.Object
{
	public class ObjectMappingRules : IMappingRules
	{
		public delegate object FactoryDelegate(object from, Type toType);

		private bool aUseImplicitMappings;
		private readonly List<IMemberMappingDefinition> aExplicitMappings;
		private FactoryDelegate aFactoryFunction;

		public ObjectMappingRules()
		{
			this.aUseImplicitMappings = true;
			this.aExplicitMappings = new List<IMemberMappingDefinition>();
			this.aFactoryFunction = null;
		}

		public ObjectMappingRules Factory(FactoryDelegate factoryFunction)
		{
			this.aFactoryFunction = factoryFunction;

			return this;
		}

		/// <summary>
		/// Marks the mapping to use only explicit column mapping.
		/// </summary>
		public ObjectMappingRules OnlyExplicit
		{
			get
			{
				this.aUseImplicitMappings = false;
				return this;
			}
		}

		public ObjectMappingRules Member(string from, string to)
		{
			SimpleMemberMappingDefinition def = new SimpleMemberMappingDefinition(from, to);
			this.aExplicitMappings.Add(def);

			return this;
		}

		public ObjectMappingRules Member<TFromType, TToType>(string from, string to, Action<MemberMappingDefinition<TFromType, TToType>> mappingDefinition)
		{
			MemberMappingDefinition<TFromType, TToType> def = new MemberMappingDefinition<TFromType, TToType>(from, to);
			mappingDefinition(def);
			this.aExplicitMappings.Add(def);

			return this;
		}

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingDefinitionInformation mappingDefinition)
		{
			Func<TFrom, TTo> factoryFunction;

			if (this.aFactoryFunction != null)
				factoryFunction = from => (TTo) this.aFactoryFunction(from, typeof(TTo));
			else
				factoryFunction = null;

			var members = this.aExplicitMappings.Select(x => x.CreateMapping(mappingDefinition, typeof(TFrom), typeof(TTo)));

			return new ObjectToObject<TFrom, TTo>(factoryFunction, mappingDefinition, members, this.aUseImplicitMappings);
		}

		#endregion
	}
}
