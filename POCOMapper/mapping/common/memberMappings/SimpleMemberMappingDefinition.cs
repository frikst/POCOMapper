using System;
using POCOMapper.conventions.members;
using POCOMapper.definition;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.common.parser;

namespace POCOMapper.mapping.common.memberMappings
{
	public class SimpleMemberMappingDefinition : IMemberMappingDefinition
	{
		private readonly string aFromName;
		private readonly string aToName;

		internal SimpleMemberMappingDefinition(string fromName, string toName)
		{
			this.aFromName = fromName;
			this.aToName = toName;
		}

		#region Implementation of IMemberMappingDefinition

		PairedMembers IMemberMappingDefinition.CreateMapping(MappingImplementation allMappings, Type fromClass, Type toClass)
		{
			MemberFromNameParser parser = new MemberFromNameParser();

			IMember memberFrom = parser.Parse(allMappings.FromConventions, fromClass, this.aFromName, false);
			IMember memberTo = parser.Parse(allMappings.ToConventions, toClass, this.aToName, true);

			IMapping mapping = allMappings.GetMapping(memberFrom.Type, memberTo.Type);

			return new PairedMembers(memberFrom, memberTo, mapping);
		}

		#endregion
	}
}
