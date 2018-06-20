using System;
using KST.POCOMapper.Conventions.MemberParsers;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Object.Parser;
using KST.POCOMapper.Members;

namespace KST.POCOMapper.Mapping.Object.MemberMappings
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
			var parser = new MemberFromNameParser();

			var memberFrom = parser.Parse(allMappings.FromConventions, fromClass, this.aFromName, false);
			var memberTo = parser.Parse(allMappings.ToConventions, toClass, this.aToName, true);

			var mapping = allMappings.GetUnresolvedMapping(memberFrom.Type, memberTo.Type);

			return new PairedMembers(memberFrom, memberTo, mapping);
		}

		#endregion
	}
}
