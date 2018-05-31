using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using KST.POCOMapper.conventions.symbol;

namespace KST.POCOMapper.conventions.members
{
	public class MemberIterator : IEnumerable<IMember>
	{
		private readonly Type aType;
		private readonly Conventions aConventions;
		private readonly IMember aParent;

		public MemberIterator(Type type, Conventions conventions, IMember parent)
		{
			this.aType = type;
			this.aConventions = conventions;
			this.aParent = parent;
		}

		#region Implementation of IEnumerable

		public IEnumerator<IMember> GetEnumerator()
		{
			foreach (Conventions child in this.aConventions.GetChildConventions())
				foreach (IMember member in child.GetAllMembers(this.aType, this.aParent))
					yield return member;

			foreach (MemberType memberType in this.aConventions.GetMemberScanningPrecedence())
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
			HashSet<Symbol> used = new HashSet<Symbol>();

			for (Type current = this.aType; current != null; current = current.BaseType)
			{
				foreach (FieldInfo field in current.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
				{
					Symbol symbol = this.aConventions.Fields.Parse(field.Name);

					if (!used.Contains(symbol))
					{
						yield return new FieldMember(this.aParent, symbol, field, this.aConventions);
						used.Add(symbol);
					}
				}
			}
		}

		private IEnumerable<IMember> GetMethods()
		{
			HashSet<Symbol> used = new HashSet<Symbol>();

			for (Type current = this.aType; current != null; current = current.BaseType)
			{
				Dictionary<Tuple<Symbol, Type>, MethodInfo[]> methodMembers = new Dictionary<Tuple<Symbol, Type>, MethodInfo[]>();

				foreach (MethodInfo method in current.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
				{
					Symbol symbol = this.aConventions.Methods.Parse(method.Name);
					if (symbol.HasPrefix("get") && method.GetParameters().Length == 0 && method.ReturnType != typeof(void))
					{
						Tuple<Symbol, Type> key = new Tuple<Symbol, Type>(symbol.GetWithoutPrefix(), method.ReturnType);
						MethodInfo[] item;
						if (methodMembers.TryGetValue(key, out item))
							item[0] = method;
						else
							methodMembers[key] = new MethodInfo[] { method, null };
					}
					else if (symbol.HasPrefix("set") && method.GetParameters().Length == 1 && method.ReturnType == typeof(void))
					{
						Tuple<Symbol, Type> key = new Tuple<Symbol, Type>(symbol.GetWithoutPrefix(), method.GetParameters()[0].ParameterType);
						MethodInfo[] item;
						if (methodMembers.TryGetValue(key, out item))
							item[1] = method;
						else
							methodMembers[key] = new MethodInfo[] { null, method };
					}
				}

				foreach (KeyValuePair<Tuple<Symbol, Type>, MethodInfo[]> method in methodMembers)
				{
					if (!used.Contains(method.Key.Item1))
					{
						yield return new MethodMember(this.aParent, method.Key.Item1, method.Value[0], method.Value[1], this.aConventions);
						used.Add(method.Key.Item1);
					}
				}
			}
		}

		private IEnumerable<IMember> GetProperties(bool codeProperties = false, bool autoProperties = false)
		{
			HashSet<Symbol> used = new HashSet<Symbol>();

			for (Type current = this.aType; current != null; current = current.BaseType)
			{
				foreach (PropertyInfo property in current.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
				{
					Symbol symbol = this.aConventions.Properties.Parse(property.Name);

					if (!used.Contains(symbol))
					{
						bool isAutoProperty = this.IsAutoProperty(property);

						if ((isAutoProperty && autoProperties) || (!isAutoProperty && codeProperties))
							yield return new PropertyMember(this.aParent, symbol, property, this.aConventions);

						used.Add(symbol);
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
