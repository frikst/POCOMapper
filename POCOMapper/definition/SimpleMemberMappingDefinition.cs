using System;
using System.Linq;
using System.Reflection;
using POCOMapper.conventions;
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
			IMember memberFrom = this.WrapMember(allMappings.FromConventions, this.GetMember(this.aFromClass, this.aFromName), false);
			IMember memberTo = this.WrapMember(allMappings.ToConventions, this.GetMember(this.aToClass, this.aToName), true);

			IMapping mapping;
			if (memberFrom.Type == memberTo.Type)
				mapping = null;
			else
				mapping = allMappings.GetMapping(memberFrom.Type, memberTo.Type);

			return new PairedMembers(memberFrom, memberTo, mapping);
		}

		#endregion

		private IMember WrapMember(Conventions conventions, MemberInfo member, bool write)
		{
			if (member is FieldInfo)
				return new FieldMember(null, conventions.Attributes.Parse(member.Name), (FieldInfo) member);
			else if (member is PropertyInfo)
				return new PropertyMember(null, conventions.Attributes.Parse(member.Name), (PropertyInfo) member);
			else
			{
				if (write)
					return new MethodMember(null, conventions.Attributes.Parse(member.Name), null, (MethodInfo) member);
				else
					return new MethodMember(null, conventions.Attributes.Parse(member.Name), (MethodInfo) member, null);
			}
		}

		private MemberInfo GetMember(Type type, string name)
		{
			MemberInfo ret = type.GetMember(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();

			if (ret == null && type.BaseType != null)
				return this.GetMember(type.BaseType, name);

			return ret;
		}
	}
}
