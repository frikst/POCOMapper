using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using POCOMapper.conventions.symbol;

namespace POCOMapper.conventions.members
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
			foreach (FieldInfo field in this.aType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				Symbol symbol = this.aConventions.Attributes.Parse(field.Name);
				yield return new FieldMember(this.aParent, symbol, field);
			}

			Dictionary<Tuple<Symbol, Type>, MethodInfo[]> methodMembers = new Dictionary<Tuple<Symbol, Type>, MethodInfo[]>();

			foreach (MethodInfo method in this.aType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
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
				yield return new MethodMember(this.aParent, method.Key.Item1, method.Value[0], method.Value[1]);

			foreach (PropertyInfo property in this.aType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				Symbol symbol = this.aConventions.Properties.Parse(property.Name);
				yield return new PropertyMember(this.aParent, symbol, property);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
