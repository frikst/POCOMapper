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
		private readonly Type aType;

		public MemberFromNameParser(Type type)
		{
			this.aType = type;
		}

		public IMember ParseRead(string name)
		{
			return this.CreateMember(this.ParseMemberString(name), false);
		}

		public IMember ParseWrite(string name)
		{
			return this.CreateMember(this.ParseMemberString(name), true);
		}

		private IMember CreateMember(IEnumerable<MemberInfo> members, bool write)
		{
			IMember previousMember = null;

			foreach (var current in members)
			{
				IMember currentMember;
				switch (current)
				{
					case FieldInfo currentField:
						currentMember = new FieldMember(previousMember, currentField);
						break;
					case PropertyInfo currentProperty:
						currentMember = new PropertyMember(previousMember, currentProperty);
						break;
					case MethodInfo currentMethod:
						if (write)
							currentMember = new MethodMember(previousMember, null, currentMethod);
						else
							currentMember = new MethodMember(previousMember, currentMethod, null);
						break;
					default:
						throw new Exception("Unkown member type");
				}

				previousMember = currentMember;
			}

			return previousMember;
		}

		private IEnumerable<MemberInfo> ParseMemberString(string path)
		{
			var curType = this.aType;

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
				var ret = curType.GetMember(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).FirstOrDefault();

				if (ret != null)
					return ret;
			}

			throw new InvalidMappingException($"{name} member not found in type {type.Name}");
		}
	}
}
