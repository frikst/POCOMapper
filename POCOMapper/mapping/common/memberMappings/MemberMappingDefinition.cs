using System;
using POCOMapper.conventions.members;
using POCOMapper.definition;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.common.parser;
using POCOMapper.mapping.special;

namespace POCOMapper.mapping.common.memberMappings
{
	public class MemberMappingDefinition<TFromType, TToType> : IMemberMappingDefinition, IRulesDefinition<TFromType, TToType>
	{
		private readonly Type aFromClass;
		private readonly Type aToClass;
		private readonly string aFromName;
		private readonly string aToName;
		private IMappingRules<TFromType, TToType> aRules;

		internal MemberMappingDefinition(Type fromClass, Type toClass, string fromName, string toName)
		{
			this.aFromClass = fromClass;
			this.aToClass = toClass;
			this.aFromName = fromName;
			this.aToName = toName;

			this.aRules = new DefaultMappingRules<TFromType, TToType>();
		}

		#region Implementation of IMemberMappingDefinition

		PairedMembers IMemberMappingDefinition.CreateMapping(MappingImplementation allMappings)
		{
			MemberFromNameParser parser = new MemberFromNameParser();

			IMapping<TFromType, TToType> mapping;

			IMember memberFrom;
			if (this.aFromName == null)
				memberFrom = new ThisMember<TFromType>();
			else
				memberFrom = parser.Parse(allMappings.FromConventions, this.aFromClass, this.aFromName, false);
			IMember memberTo;
			if (this.aToName == null)
				memberTo = new ThisMember<TToType>();
			else
				memberTo = parser.Parse(allMappings.ToConventions, this.aToClass, this.aToName, true);

			mapping = this.aRules.Create(allMappings);

			return new PairedMembers(memberFrom, memberTo, mapping);
		}

		#endregion

		#region Implementation of IRulesDefinition<TFromType,TToType>

		public TRules Rules<TRules>()
			where TRules : class, IMappingRules<TFromType, TToType>, new()
		{
			TRules ret = new TRules();
			this.aRules = ret;
			return ret;
		}

		#endregion
	}
}