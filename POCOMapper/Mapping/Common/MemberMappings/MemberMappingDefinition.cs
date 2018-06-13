using System;
using KST.POCOMapper.Conventions.MemberParsers;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Common.Parser;
using KST.POCOMapper.Members;

namespace KST.POCOMapper.Mapping.Common.MemberMappings
{
	public class MemberMappingDefinition<TFromType, TToType> : IMemberMappingDefinition, IRulesDefinition<TFromType, TToType>
	{
		private readonly string aFromName;
		private readonly string aToName;
		private IMappingRules<TFromType, TToType> aRules;

		internal MemberMappingDefinition(string fromName, string toName)
		{
			this.aFromName = fromName;
			this.aToName = toName;

			this.aRules = new DefaultMappingRules<TFromType, TToType>();
		}

		#region Implementation of IMemberMappingDefinition

		PairedMembers IMemberMappingDefinition.CreateMapping(MappingImplementation allMappings, Type fromClass, Type toClass)
		{
			MemberFromNameParser parser = new MemberFromNameParser();

			IMapping<TFromType, TToType> mapping;

			IMember memberFrom;
			if (this.aFromName == null)
				memberFrom = new ThisMember<TFromType>();
			else
				memberFrom = parser.Parse(allMappings.FromConventions, fromClass, this.aFromName, false);
			IMember memberTo;
			if (this.aToName == null)
				memberTo = new ThisMember<TToType>();
			else
				memberTo = parser.Parse(allMappings.ToConventions, toClass, this.aToName, true);

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