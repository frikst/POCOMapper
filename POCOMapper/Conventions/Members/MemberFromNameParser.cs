using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KST.POCOMapper.Exceptions;

namespace KST.POCOMapper.Conventions.Members
{
	public class MemberFromNameParser
	{
		public IMember Parse(NamingConventions conventions, Type type, string name, bool write)
		{
			return this.WrapMember(conventions, this.GetMember(type, name), write);
		}

		private IMember WrapMember(NamingConventions conventions, Stack<MemberInfo> members, bool write)
		{
			MemberInfo current = members.Pop();
			IMember parent;
			if (members.Count > 0)
				parent = this.WrapMember(conventions, members, false);
			else
				parent = null;

			switch (current)
			{
				case FieldInfo currentField:
					return new FieldMember(parent, conventions.Fields.Parse(currentField.Name), currentField, conventions);
				case PropertyInfo currentProperty:
					return new PropertyMember(parent, conventions.Fields.Parse(currentProperty.Name), currentProperty, conventions);
				case MethodInfo currentMethod:
					if (write)
						return new MethodMember(parent, conventions.Fields.Parse(current.Name), null, currentMethod, conventions);
					else
						return new MethodMember(parent, conventions.Fields.Parse(current.Name), currentMethod, null, conventions);
				default:
					throw new Exception("Unkown member type");
			}
		}

		private Stack<MemberInfo> GetMember(Type type, string path)
		{
			string[] names = path.Split('.');
			Stack<MemberInfo> ret = new Stack<MemberInfo>();

			foreach (string name in names)
			{
				MemberInfo cur = this.GetOneMember(type, name);

				switch (cur)
				{
					case PropertyInfo curProperty:
						type = curProperty.PropertyType;
						break;
					case FieldInfo curField:
						type = curField.FieldType;
						break;
					case MethodInfo curMethod:
						type = curMethod.ReturnType;
						break;
					case null:
						throw new InvalidMappingException($"{name} member not found in type {type.Name}");
					default:
						throw new Exception("Unkown member type");
				}

				ret.Push(cur);
			}

			return ret;
		}

		private MemberInfo GetOneMember(Type type, string name)
		{
			MemberInfo ret = type.GetMember(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();

			if (ret == null && type.BaseType != null)
				return this.GetOneMember(type.BaseType, name);

			return ret;
		}
	}
}
