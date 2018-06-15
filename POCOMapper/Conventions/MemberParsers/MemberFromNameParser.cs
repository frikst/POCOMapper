using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Members;

namespace KST.POCOMapper.Conventions.MemberParsers
{
	internal class MemberFromNameParser
	{
		public IMember Parse(NamingConventions conventions, Type type, string name, bool write)
		{
			return this.CreateMember(conventions, this.ParseMemberString(type, name), write);
		}

		private IMember CreateMember(NamingConventions conventions, IEnumerable<MemberInfo> members, bool write)
		{
			IMember previousMember = null;

			foreach (var current in members)
			{
				IMember currentMember;
				switch (current)
				{
					case FieldInfo currentField:
						currentMember = new FieldMember(previousMember, conventions.Fields.Parse(currentField.Name), currentField, conventions);
						break;
					case PropertyInfo currentProperty:
						currentMember = new PropertyMember(previousMember, conventions.Properties.Parse(currentProperty.Name), currentProperty, conventions);
						break;
					case MethodInfo currentMethod:
						if (write)
							currentMember = new MethodMember(previousMember, conventions.Methods.Parse(current.Name), null, currentMethod, conventions);
						else
							currentMember = new MethodMember(previousMember, conventions.Methods.Parse(current.Name), currentMethod, null, conventions);
						break;
					default:
						throw new Exception("Unkown member type");
				}

				previousMember = currentMember;
			}

			return previousMember;
		}

		private IEnumerable<MemberInfo> ParseMemberString(Type type, string path)
		{
			var curType = type;

			foreach (var name in path.Split('.'))
			{
				var cur = this.GetMember(curType, name);

				switch (cur)
				{
					case PropertyInfo curProperty:
						curType = curProperty.PropertyType;
						break;
					case FieldInfo curField:
						curType = curField.FieldType;
						break;
					case MethodInfo curMethod:
						curType = curMethod.ReturnType;
						break;
					case null:
						throw new InvalidMappingException($"{name} member not found in type {curType.Name}");
					default:
						throw new Exception("Unkown member type");
				}

				yield return cur;
			}
		}

		private MemberInfo GetMember(Type type, string name)
		{
			for (var curType = type; curType != null; curType = curType.BaseType)
			{
				var ret = curType.GetMember(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();

				if (ret != null)
					return ret;
			}

			return null;
		}
	}
}
