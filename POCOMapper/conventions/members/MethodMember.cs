using System;
using System.Linq.Expressions;
using System.Reflection;
using POCOMapper.conventions.symbol;

namespace POCOMapper.conventions.members
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

		public Symbol Symbol { get; private set; }

		public Type Type
		{
			get { return this.aGetMethod.ReturnType; }
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
	}
}
