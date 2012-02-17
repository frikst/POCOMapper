using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using POCOMapper.exceptions;

namespace POCOMapper.conventions.members
{
	public class MemberFromNameParser
	{
		public IMember Parse(Conventions conventions, Type type, string name, bool write)
		{
			return this.WrapMember(conventions, this.GetMember(type, name), write);
		}

		private IMember WrapMember(Conventions conventions, Stack<MemberInfo> members, bool write)
		{
			MemberInfo current = members.Pop();
			IMember parent;
			if (members.Count > 0)
				parent = this.WrapMember(conventions, members, false);
			else
				parent = null;

			if (current is FieldInfo)
				return new FieldMember(parent, conventions.Attributes.Parse(current.Name), (FieldInfo)current);
			else if (current is PropertyInfo)
				return new PropertyMember(parent, conventions.Attributes.Parse(current.Name), (PropertyInfo)current);
			else
			{
				if (write)
					return new MethodMember(parent, conventions.Attributes.Parse(current.Name), null, (MethodInfo)current);
				else
					return new MethodMember(parent, conventions.Attributes.Parse(current.Name), (MethodInfo)current, null);
			}
		}

		private Stack<MemberInfo> GetMember(Type type, string path)
		{
			string[] names = path.Split('.');
			Stack<MemberInfo> ret = new Stack<MemberInfo>();

			foreach (string name in names)
			{
				MemberInfo cur = this.GetOneMember(type, name);

				if (cur == null)
					throw new InvalidMapping(string.Format("{0} member not found in type {1}", name, type.Name));
				else if (cur is PropertyInfo)
					type = ((PropertyInfo)cur).PropertyType;
				else if (cur is FieldInfo)
					type = ((FieldInfo)cur).FieldType;
				else
					type = ((MethodInfo)cur).ReturnType;

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
