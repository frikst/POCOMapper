using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using POCOMapper.conventions.symbol;
using POCOMapper.definition;
using POCOMapper.mapping.@base;

namespace POCOMapper.conventions.parser
{
	public class TypePairParser : IEnumerable<PairedMembers>
	{
		private readonly MappingImplementation aMapping;
		private readonly Type aFrom;
		private readonly Type aTo;

		public TypePairParser(MappingImplementation mapping, Type from, Type to)
		{
			aMapping = mapping;
			aFrom = from;
			aTo = to;
		}

		private IEnumerable<IMember> FindMembers(Type obj, Conventions conventions)
		{
			foreach (FieldInfo field in obj.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				Symbol symbol = conventions.Attributes.Parse(field.Name);
				yield return new FieldMember(null, symbol, field);
			}

			Dictionary<Tuple<Symbol, Type>, MethodInfo[]> methodMembers = new Dictionary<Tuple<Symbol, Type>, MethodInfo[]>();

			foreach (MethodInfo method in obj.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				Symbol symbol = conventions.Methods.Parse(method.Name);
				if (symbol.HasPrefix("get") && method.GetParameters().Length == 0 && method.ReturnType != typeof(void))
				{
					Tuple<Symbol, Type> key = new Tuple<Symbol,Type>(symbol.GetWithoutPrefix(), method.ReturnType);
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
				yield return new MethodMember(null, method.Key.Item1, method.Value[0], method.Value[1]);

			foreach (PropertyInfo property in obj.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				Symbol symbol = conventions.Properties.Parse(property.Name);
				yield return new PropertyMember(null, symbol, property);
			}
		}

		private PairedMembers DetectPair(IMember from, IMember to)
		{
			if (from.Symbol == to.Symbol)
			{
				if (from.Type == to.Type)
					return new PairedMembers(from, to, null);
				else
				{
					IMapping mapping = this.aMapping.GetMapping(from.Type, to.Type);
					if (mapping == null)
						return new PairedMembers(from, to, mapping);
					else
						return null;
				}
			}

			return null;
		}

		#region Implementation of IEnumerable

		public IEnumerator<PairedMembers> GetEnumerator()
		{
			List<IMember> fromAll = this.FindMembers(this.aFrom, this.aMapping.FromConventions).Where(x => x.Getter != null).ToList();
			List<IMember> toAll = this.FindMembers(this.aTo, this.aMapping.ToConventions).Where(x => x.Setter != null).ToList();

			foreach (IMember fromOne in fromAll)
			{
				foreach (IMember toOne in toAll)
				{
					PairedMembers pair = this.DetectPair(fromOne, toOne);

					if (pair != null)
					{
						yield return pair;
						break;
					}
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
