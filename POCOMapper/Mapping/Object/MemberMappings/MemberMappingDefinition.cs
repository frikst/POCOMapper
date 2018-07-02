using System;
using KST.POCOMapper.Conventions.MemberParsers;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Object.Parser;
using KST.POCOMapper.Members;

namespace KST.POCOMapper.Mapping.Object.MemberMappings
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

			this.aRules = null;
		}

		#region Implementation of IMemberMappingDefinition

		PairedMembers IMemberMappingDefinition.CreateMapping(MappingDefinitionInformation mappingDefinition, Type fromClass, Type toClass)
		{
			IMember memberFrom;
			if (this.aFromName == null)
				memberFrom = new ThisMember<TFromType>();
			else
				memberFrom = new MemberFromNameParser(fromClass).ParseRead(this.aFromName);
			IMember memberTo;
			if (this.aToName == null)
				memberTo = new ThisMember<TToType>();
			else
				memberTo = new MemberFromNameParser(toClass).ParseWrite(this.aToName);

			IUnresolvedMapping<TFromType, TToType> mapping;
			if (this.aRules == null)
				mapping = mappingDefinition.UnresolvedMappings.GetUnresolvedMapping<TFromType, TToType>();
			else
				mapping = this.aRules.Create(mappingDefinition).AsUnresolved();

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