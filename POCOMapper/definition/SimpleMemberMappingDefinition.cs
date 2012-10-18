using System;
using POCOMapper.conventions.members;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.common.parser;

namespace POCOMapper.definition
{
	public class SimpleMemberMappingDefinition : IMemberMappingDefinition
	{
		private readonly Type aFromClass;
		private readonly Type aToClass;
		private readonly string aFromName;
		private readonly string aToName;

		internal SimpleMemberMappingDefinition(Type fromClass, Type toClass, string fromName, string toName)
		{
			this.aFromClass = fromClass;
			this.aToClass = toClass;

			this.aFromName = fromName;
			this.aToName = toName;
		}

		#region Implementation of IMemberMappingDefinition

		PairedMembers IMemberMappingDefinition.CreateMapping(MappingImplementation allMappings)
		{
			MemberFromNameParser parser = new MemberFromNameParser();

			IMember memberFrom = parser.Parse(allMappings.FromConventions, this.aFromClass, this.aFromName, false);
			IMember memberTo = parser.Parse(allMappings.ToConventions, this.aToClass, this.aToName, true);

			IMapping mapping = allMappings.GetMapping(memberFrom.Type, memberTo.Type);

			return new PairedMembers(memberFrom, memberTo, mapping);
		}

		#endregion
	}
}
