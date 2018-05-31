using System;
using System.Linq.Expressions;
using System.Reflection;
using POCOMapper.conventions.symbol;

namespace POCOMapper.conventions.members
{
	public class FieldMember : IMember
	{
		private readonly FieldInfo aField;
		private readonly Conventions aConventions;

		public FieldMember(IMember parent, Symbol symbol, FieldInfo field, Conventions conventions)
		{
			this.Parent = parent;

			this.Symbol = symbol;

			this.aField = field;

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
			get { return this.aField.FieldType; }
		}

		public Type DeclaringType
		{
			get { return this.aField.DeclaringType; }
		}

		public MemberInfo Setter
		{
			get { return this.aField; }
		}

		public string Name
		{
			get { return this.aField.Name; }
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

		public MemberInfo Getter
		{
			get { return this.aField; }
		}

		public bool CanPairWith(IMember other)
		{
			return this.aConventions.CanPair(this, other);
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

		public bool Equals(FieldMember other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other.aField, this.aField) && Equals(other.Parent, this.Parent);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (FieldMember)) return false;
			return Equals((FieldMember) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((this.aField != null ? this.aField.GetHashCode() : 0)*397) ^ (this.Parent != null ? this.Parent.GetHashCode() : 0);
			}
		}
	}
}
