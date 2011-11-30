using System;
using System.Linq.Expressions;
using System.Reflection;
using POCOMapper.conventions.symbol;

namespace POCOMapper.conventions.members
{
	public class FieldMember : IMember
	{
		private readonly FieldInfo aField;

		public FieldMember(IMember parent, Symbol symbol, FieldInfo field)
		{
			this.Parent = parent;

			this.Symbol = symbol;

			this.aField = field;
		}

		#region Implementation of IMember

		public IMember Parent { get; private set; }

		public Symbol Symbol { get; set; }

		public Type Type
		{
			get { return this.aField.FieldType; }
		}

		public MemberInfo Setter
		{
			get { return this.aField; }
		}

		public MemberInfo Getter
		{
			get { return this.aField; }
		}

		public Expression CreateGetterExpression(ParameterExpression parentVariable)
		{
			return Expression.Field(parentVariable, this.aField);
		}

		public Expression CreateSetterExpression(ParameterExpression parentVariable, Expression value)
		{
			return Expression.Assign(Expression.Field(parentVariable, this.aField), value);
		}

		#endregion

		public override string ToString()
		{
			if (this.Parent != null)
				return string.Format("{0}.[F]{1}", this.Parent, this.Symbol);
			return string.Format("[F]{0}", this.Symbol);
		}
	}
}
