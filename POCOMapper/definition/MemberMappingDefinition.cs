using System;
using System.Linq;
using System.Reflection;
using POCOMapper.conventions;
using POCOMapper.conventions.members;
using POCOMapper.exceptions;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.common.parser;
using POCOMapper.mapping.special;

namespace POCOMapper.definition
{
	public class MemberMappingDefinition<TFromType, TToType> : IMemberMappingDefinition
	{
		private readonly Type aFromClass;
		private readonly Type aToClass;
		private readonly string aFromName;
		private readonly string aToName;
		private Type aMapping;
		private Func<TFromType, TToType> aMappingFunc;
		private Action<TFromType, TToType> aMappingAction;

		internal MemberMappingDefinition(Type fromClass, Type toClass, string fromName, string toName)
		{
			this.aFromClass = fromClass;
			this.aToClass = toClass;
			this.aFromName = fromName;
			this.aToName = toName;

			this.aMapping = null;
			this.aMappingAction = null;
			this.aMappingFunc = null;
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

			if (this.aMapping != null)
				mapping = (IMapping<TFromType, TToType>)Activator.CreateInstance(this.aMapping, allMappings);
			else if (this.aMappingFunc != null)
				mapping = new FuncMapping<TFromType, TToType>(this.aMappingFunc);
			else if (this.aMappingAction != null)
				mapping = new FuncMapping<TFromType, TToType>(this.aMappingAction);
			else
				mapping = allMappings.GetMapping<TFromType, TToType>();

			return new PairedMembers(memberFrom, memberTo, mapping);
		}

		#endregion

		public void Using<TMapping>()
			where TMapping : IMapping<TFromType, TToType>
		{
			this.aMapping = typeof(TMapping);
		}

		public void Using(Func<TFromType, TToType> mappingFunc)
		{
			this.aMappingFunc = mappingFunc;
		}

		public void Using(Action<TFromType, TToType> mappingAction)
		{
			this.aMappingAction = mappingAction;
		}
	}
}