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

		public MethodMember(IMember parent, Symbol symbol, MethodInfo getMethod, MethodInfo setMethod)
		{
			this.Parent = parent;

			this.Symbol = symbol;

			this.aSetMethod = setMethod;
			this.aGetMethod = getMethod;
		}

		#region Implementation of IMember

		public IMember Parent { get; private set; }

		public Symbol Symbol { get; set; }

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
	}
}
