using System;
using System.Linq.Expressions;
using System.Reflection;
using KST.POCOMapper.Conventions;

namespace KST.POCOMapper.Members
{
	public class MethodMember : IMember
	{
		private readonly MethodInfo aGetMethod;
		private readonly MethodInfo aSetMethod;
		private readonly NamingConventions aConventions;

		public MethodMember(IMember parent, Symbol symbol, MethodInfo getMethod, MethodInfo setMethod, NamingConventions conventions)
		{
			this.Parent = parent;

			this.Symbol = symbol;

			this.aSetMethod = setMethod;
			this.aGetMethod = getMethod;

			this.aConventions = conventions;
		}

		public MethodMember(IMember parent, MethodInfo getMethod, MethodInfo setMethod)
		{
			this.Parent = parent;

			this.Symbol = Symbol.Undefined;

			this.aSetMethod = setMethod;
			this.aGetMethod = getMethod;

			this.aConventions = null;
		}

		public MethodInfo GetMethod
			=> this.aGetMethod;

		public MethodInfo SetMethod
			=> this.aSetMethod;

		#region Implementation of IMember

		public IMember Parent { get; }

		public Symbol Symbol { get; }

		public Type Type
			=> this.aGetMethod.ReturnType;

		public Type DeclaringType
			=> this.aGetMethod.DeclaringType;

		public string Name
			=> this.aGetMethod.Name;

		public bool Readable
			=> this.aGetMethod != null;

		public bool Writable
			=> this.aSetMethod != null;

		public bool CanPairWith(IMember other)
			=> this.aConventions?.CanPair(this, other) ?? true;

		public Expression CreateGetterExpression(ParameterExpression parentVariable)
		{
			if (this.aGetMethod != null)
				return Expression.Call(parentVariable, this.aGetMethod);
			return null;
		}

		public Expression CreateSetterExpression(ParameterExpression parentVariable, Expression value)
		{
			if (this.aSetMethod != null)
				return Expression.Call(parentVariable, this.aSetMethod, value);
			return null;
		}

		#endregion

		public override string ToString()
		{
			if (this.Parent != null)
				return $"{this.Parent}.[M]{this.Name}";
			return $"[M]{this.Name}";
		}

		public override bool Equals(object obj)
		{
			if (obj is MethodMember methodObj)
				return methodObj.aGetMethod == this.aGetMethod && methodObj.aSetMethod == this.aSetMethod && Equals(methodObj.Parent, this.Parent);

			return false;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((((this.aGetMethod?.GetHashCode() ?? 0) * 397) ^ (this.aSetMethod?.GetHashCode() ?? 0)) * 397) ^ (this.Parent?.GetHashCode() ?? 0);
			}
		}
	}
}
