using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using KST.POCOMapper.Members;

namespace KST.POCOMapper.Conventions.MemberParsers
{
	internal class MemberIterator : IEnumerable<IMember>
	{
		private readonly Type aType;
		private readonly NamingConventions aConventions;
		private readonly IMember aParent;

		public MemberIterator(Type type, NamingConventions conventions, IMember parent)
		{
			this.aType = type;
			this.aConventions = conventions;
			this.aParent = parent;
		}

		#region Implementation of IEnumerable

		public IEnumerator<IMember> GetEnumerator()
		{
			foreach (NamingConventions child in this.aConventions.GetChildConventions())
				foreach (IMember member in child.GetAllMembers(this.aType, this.aParent))
					yield return member;

			foreach (MemberType memberType in this.aConventions.MemberScanningPrecedence)
			{
				switch (memberType)
				{
					case MemberType.Field:
						foreach (IMember member in this.GetFields())
							yield return member;
						break;
					case MemberType.Method:
						foreach (IMember member in this.GetMethods())
							yield return member;
						break;
					case MemberType.Property:
						foreach (IMember member in this.GetProperties(codeProperties: true, autoProperties: true))
							yield return member;
						break;
					case MemberType.AutoProperty:
						foreach (IMember member in this.GetProperties(autoProperties: true))
							yield return member;
						break;
					case MemberType.CodeProperty:
						foreach (IMember member in this.GetProperties(codeProperties: true))
							yield return member;
						break;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		private IEnumerable<IMember> GetFields()
		{
			HashSet<string> used = new HashSet<string>();

			for (Type current = this.aType; current != null && current != typeof(object); current = current.BaseType)
			{
				foreach (FieldInfo field in current.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
				{
					if (field.IsSpecialName || field.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any() || field.Name.Contains('.'))
						continue;

					Symbol symbol = this.aConventions.Fields.Parse(field.Name);

					if (used.Add(field.Name))
                        yield return new FieldMember(this.aParent, symbol, field, this.aConventions);
                }
			}
		}

		private IEnumerable<IMember> GetMethods()
		{
			HashSet<string> usedGetters = new HashSet<string>();
			HashSet<string> usedSetters = new HashSet<string>();

			for (Type current = this.aType; current != null && current != typeof(object); current = current.BaseType)
			{
				var methodMembers = new Dictionary<(Symbol MemberName, Type MemberType), (MethodInfo Getter, MethodInfo Setter)>();

				foreach (MethodInfo method in current.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
				{
					if (method.IsSpecialName || method.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any() || method.Name.Contains('.'))
						continue;

					Symbol symbol = this.aConventions.Methods.Parse(method.Name);

					if (symbol.HasPrefix("get") && method.GetParameters().Length == 0 && method.ReturnType != typeof(void))
					{
						var key = (symbol.GetWithoutPrefix(), method.ReturnType);
						if (methodMembers.TryGetValue(key, out var item))
							methodMembers[key] = (method, item.Setter);
						else
							methodMembers[key] = (method, null);
					}
					else if (symbol.HasPrefix("set") && method.GetParameters().Length == 1 && method.ReturnType == typeof(void))
					{
						var key = (symbol.GetWithoutPrefix(), method.GetParameters()[0].ParameterType);
						if (methodMembers.TryGetValue(key, out var item))
							methodMembers[key] = (item.Setter, method);
						else
							methodMembers[key] = (null, method);
					}
				}

				foreach (var methodPair in methodMembers)
				{
					if ((methodPair.Value.Getter == null || usedGetters.Add(methodPair.Value.Getter.Name)) && (methodPair.Value.Setter == null || usedSetters.Add(methodPair.Value.Setter.Name)))
						yield return new MethodMember(this.aParent, methodPair.Key.MemberName, methodPair.Value.Getter, methodPair.Value.Setter, this.aConventions);
				}
			}
		}

		private IEnumerable<IMember> GetProperties(bool codeProperties = false, bool autoProperties = false)
		{
			HashSet<string> used = new HashSet<string>();

			for (Type current = this.aType; current != null && current != typeof(object); current = current.BaseType)
			{
				foreach (PropertyInfo property in current.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
				{
					if (property.IsSpecialName || property.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any() || property.Name.Contains('.'))
						continue;

					Symbol symbol = this.aConventions.Properties.Parse(property.Name);

					if (used.Add(property.Name))
					{
						bool isAutoProperty = this.IsAutoProperty(property);

						if ((isAutoProperty && autoProperties) || (!isAutoProperty && codeProperties))
							yield return new PropertyMember(this.aParent, symbol, property, this.aConventions);
					}
				}
			}
		}

		private bool IsAutoProperty(PropertyInfo property)
		{
			if (property.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any())
				return false;

			var getter = property.GetGetMethod(true);
			var setter = property.GetSetMethod(true);

			if (getter == null)
				return false;

			if (setter == null)
				return false;

			if (!getter.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any())
				return false;
			if (!setter.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any())
				return false;

			return true;
		}
	}
}
