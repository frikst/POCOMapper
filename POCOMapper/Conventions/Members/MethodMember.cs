using System;
using System.Linq.Expressions;
using System.Reflection;
using KST.POCOMapper.conventions.symbol;

namespace KST.POCOMapper.conventions.members
{
	public class MethodMember : IMember
	{
		private readonly MethodInfo aGetMethod;
		private readonly MethodInfo aSetMethod;
		private readonly Conventions aConventions;

		public MethodMember(IMember parent, Symbol symbol, MethodInfo getMethod, MethodInfo setMethod, Conventions conventions)
		{
			this.Parent = parent;

			this.Symbol = symbol;

			this.aSetMethod = setMethod;
			this.aGetMethod = getMethod;

			this.aConventions = conventions;
		}

		#region Implementation of IMember

		public IMember Parent { get; private set; }

		public int Depth
		{
			get
			{
				if (this.Parent == null)
					return 0;
				else
					return this.Parent.Depth + 1;
			}
		}

		public Symbol Symbol { get; private set; }

		public Type Type
		{
			get { return this.aGetMethod.ReturnType; }
		}

		public Type DeclaringType
		{
			get { return this.aGetMethod.DeclaringType; }
		}

		public MemberInfo Getter
		{
			get { return this.aGetMethod; }
		}

		public MemberInfo Setter
		{
			get { return this.aSetMethod; }
		}

		public string Name
		{
			get { return this.Getter.Name; }
		}

		public string FullName
		{
			get
			{
				if (this.Parent == null)
					return this.Name;
				else
					return this.Parent.Name + "." + this.Name;
			}
		}

		public bool CanPairWith(IMember other)
		{
			return this.aConventions.CanPair(this, other);
		}

		public Expression CreateGetterExpression(ParameterExpression parentVariable)
		{
			if (this.Getter != null)
				return Expression.Call(parentVariable, this.aGetMethod);
			return null;
		}

		public Expression CreateSetterExpression(ParameterExpression parentVariable, Expression value)
		{
			if (this.Setter != null)
				return Expression.Call(parentVariable, this.aSetMethod, value);
			return null;
		}

		#endregion

		public override string ToString()
		{
			if (this.Parent != null)
				return string.Format("{0}.[M]{1}", this.Parent, this.Symbol);
			return string.Format("[M]{0}", this.Symbol);
		}

		public bool Equals(MethodMember other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other.aGetMethod, this.aGetMethod) && Equals(other.aSetMethod, this.aSetMethod) && Equals(other.Parent, this.Parent);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (MethodMember)) return false;
			return this.Equals((MethodMember) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = (this.aGetMethod != null ? this.aGetMethod.GetHashCode() : 0);
				result = (result*397) ^ (this.aSetMethod != null ? this.aSetMethod.GetHashCode() : 0);
				result = (result*397) ^ (this.Parent != null ? this.Parent.GetHashCode() : 0);
				return result;
			}
		}
	}
}
