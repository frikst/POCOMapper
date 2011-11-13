using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using POCOMapper.conventions;
using POCOMapper.definition;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.common
{
	public class ObjectToObject<TFrom, TTo> : CompiledMapping<TFrom, TTo>
	{
		public ObjectToObject(MappingImplementation mapping)
			: base(mapping)
		{

		}

		public override IEnumerable<Tuple<string, IMapping>> Children
		{
			get
			{
				foreach (var mapping in PairMembers(this.GetFromGetters(), this.GetToSetters()))
					yield return new Tuple<string, IMapping>(string.Format("{0} => {1}", mapping.Item1.Name, mapping.Item2.Name), mapping.Item3);
			}
		}

		protected override Expression<Func<TFrom, TTo>> Compile()
		{
			IEnumerable<Tuple<MemberInfo, MemberInfo, IMapping, Type, Type>> pairs = PairMembers(this.GetFromGetters(), this.GetToSetters());

			ConstructorInfo constructor = typeof(TTo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { }, null);

			ParameterExpression from = Expression.Parameter(typeof(TFrom), "from");
			ParameterExpression to = Expression.Parameter(typeof(TTo), "to");

			LabelTarget fncEnd = Expression.Label();

			return Expression.Lambda<Func<TFrom, TTo>>(
				Expression.Block(
					new ParameterExpression[] { to },
					new Expression[]
					{
						Expression.Assign(
							to,
							Expression.New(constructor)
						),
						this.MakeBlock(pairs.Select(x => this.PairToExpression(x, from, to))),
						to
					}
				),
				from
			);
		}

		protected IEnumerable<Tuple<Symbol, Type, MemberInfo>> GetFromGetters()
		{
			foreach (FieldInfo field in typeof(TFrom).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				Symbol symbol = this.Mapping.FromConventions.Attributes.Parse(field.Name);
				yield return new Tuple<Symbol, Type, MemberInfo>(symbol, field.FieldType, field);
			}

			foreach (MethodInfo method in typeof(TFrom).GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				Symbol symbol = this.Mapping.FromConventions.Methods.Parse(method.Name);
				if (symbol.HasPrefix("get") && method.GetParameters().Length == 0 && method.ReturnType != typeof(void))
					yield return new Tuple<Symbol, Type, MemberInfo>(symbol.GetWithoutPrefix(), method.ReturnType, method);
			}

			foreach (PropertyInfo property in typeof(TFrom).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
				if (property.CanRead)
				{
					Symbol symbol = this.Mapping.FromConventions.Properties.Parse(property.Name);
					yield return new Tuple<Symbol, Type, MemberInfo>(symbol, property.PropertyType, property);
				}
		}

		protected IEnumerable<Tuple<Symbol, Type, MemberInfo>> GetToSetters()
		{
			foreach (FieldInfo field in typeof(TTo).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				Symbol symbol = this.Mapping.ToConventions.Attributes.Parse(field.Name);
				yield return new Tuple<Symbol, Type, MemberInfo>(symbol, field.FieldType, field);
			}

			foreach (MethodInfo method in typeof(TTo).GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				Symbol symbol = this.Mapping.ToConventions.Methods.Parse(method.Name);
				if (symbol.HasPrefix("set") && method.GetParameters().Length == 1 && method.ReturnType == typeof(void))
					yield return new Tuple<Symbol, Type, MemberInfo>(symbol.GetWithoutPrefix(), method.GetParameters()[0].ParameterType, method);
			}

			foreach (PropertyInfo property in typeof(TTo).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
				if (property.CanWrite)
				{
					Symbol symbol = this.Mapping.ToConventions.Properties.Parse(property.Name);
					yield return new Tuple<Symbol, Type, MemberInfo>(symbol, property.PropertyType, property);
				}
		}

		private IEnumerable<Tuple<MemberInfo, MemberInfo, IMapping, Type, Type>> PairMembers(IEnumerable<Tuple<Symbol, Type, MemberInfo>> from, IEnumerable<Tuple<Symbol, Type, MemberInfo>> to)
		{
			List<Tuple<Symbol, Type, MemberInfo>> fromAll = from.ToList();
			List<Tuple<Symbol, Type, MemberInfo>> toAll = to.ToList();

			foreach (Tuple<Symbol, Type, MemberInfo> fromOne in fromAll)
			{
				foreach (Tuple<Symbol, Type, MemberInfo> toOne in toAll)
				{
					IMapping mapping;

					if (this.DetectPair(fromOne, toOne, out mapping))
					{
						yield return new Tuple<MemberInfo, MemberInfo, IMapping, Type, Type>(fromOne.Item3, toOne.Item3, mapping, fromOne.Item2, toOne.Item2);
						break;
					}
				}
			}
		}

		private bool DetectPair(Tuple<Symbol, Type, MemberInfo> from, Tuple<Symbol, Type, MemberInfo> to, out IMapping mapping)
		{
			if (from.Item1 == to.Item1)
			{
				if (from.Item2 == to.Item2)
				{
					mapping = null;
					return true;
				}
				else
				{
					mapping = this.Mapping.GetMapping(from.Item2, to.Item2);
					if (mapping == null)
						return false;
					else
						return true;
				}
			}

			mapping = null;
			return false;
		}

		private Expression PairToExpression(Tuple<MemberInfo, MemberInfo, IMapping, Type, Type> pair, ParameterExpression from, ParameterExpression to)
		{
			Expression ret;

			if (pair.Item1 is MethodInfo)
				ret = Expression.Call(from, (MethodInfo)pair.Item1);
			else if (pair.Item1 is PropertyInfo)
				ret = Expression.Property(from, (PropertyInfo)pair.Item1);
			else
				ret = Expression.Field(from, (FieldInfo)pair.Item1);

			if (pair.Item3 != null)
				ret = Expression.Call(
					Expression.Constant(pair.Item3),
					typeof(IMapping<,>).MakeGenericType(pair.Item4, pair.Item5).GetMethod("Map"),
					ret
				);

			if (pair.Item2 is MethodInfo)
				ret = Expression.Call(to, (MethodInfo)pair.Item2, ret);
			else if (pair.Item2 is PropertyInfo)
				ret = Expression.Assign(Expression.Property(to, (PropertyInfo)pair.Item2), ret);
			else
				ret = Expression.Assign(Expression.Field(to, (FieldInfo)pair.Item2), ret);

			return ret;
		}

		private Expression MakeBlock(IEnumerable<Expression> expressions)
		{
			Expression[] retExpressions = expressions.ToArray();

			if (retExpressions.Length == 0)
				return Expression.Empty();

			return Expression.Block(retExpressions);
		}
	}
}
