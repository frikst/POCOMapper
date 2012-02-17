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
			IMapping<TFromType, TToType> mapping;

			IMember memberFrom;
			if (this.aFromName == null)
				memberFrom = new ThisMember<TFromType>();
			else
				memberFrom = this.WrapMember(allMappings.FromConventions, this.GetMember(this.aFromClass, this.aFromName), false);
			IMember memberTo;
			if (this.aToName == null)
				memberTo = new ThisMember<TToType>();
			else
				memberTo = this.WrapMember(allMappings.ToConventions, this.GetMember(this.aToClass, this.aToName), true);


			if (this.aMapping != null)
				mapping = (IMapping<TFromType, TToType>)Activator.CreateInstance(this.aMapping, allMappings);
			else if (this.aMappingFunc != null)
				mapping = new FuncMapping<TFromType, TToType>(this.aMappingFunc);
			else if (this.aMappingAction != null)
				mapping = new FuncMapping<TFromType, TToType>(this.aMappingAction);
			else if (memberFrom.Type == memberTo.Type)
				mapping = null;
			else
				mapping = allMappings.GetMapping<TFromType, TToType>();

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

		public void Using<TMapping>()
			where TMapping : IMapping<TFromType, TToType>
		{
			this.aMapping = typeof(TMapping);
		}

		public void Using(Func<TFromType, TToType> mappingFunc)
		{
			if (this.aToName == null)
				throw new InvalidMapping("Cannot use Func<,> to define mapping when target is not member");
			this.aMappingFunc = mappingFunc;
		}

		public void Using(Action<TFromType, TToType> mappingAction)
		{
			if (this.aToName != null)
				throw new InvalidMapping("Cannot use Action<,> to define mapping when target is member");
			this.aMappingAction = mappingAction;
		}
	}
}