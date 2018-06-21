using System;
using KST.POCOMapper.Conventions.MemberParsers;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Object.Parser;

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

		PairedMembers IMemberMappingDefinition.CreateMapping(MappingDefinitionInformation mappingDefinition, Type fromClass, Type toClass)
		{
			var parser = new MemberFromNameParser();

			var memberFrom = parser.Parse(mappingDefinition.FromConventions, fromClass, this.aFromName, false);
			var memberTo = parser.Parse(mappingDefinition.ToConventions, toClass, this.aToName, true);

			var mapping = mappingDefinition.UnresolvedMappings.GetUnresolvedMapping(memberFrom.Type, memberTo.Type);

			return new PairedMembers(memberFrom, memberTo, mapping);
		}

		#endregion
	}
}
