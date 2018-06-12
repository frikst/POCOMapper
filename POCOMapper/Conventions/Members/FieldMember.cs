using System;
using System.Linq.Expressions;
using System.Reflection;
using KST.POCOMapper.Conventions.Symbols;

namespace KST.POCOMapper.Conventions.Members
{
	public class FieldMember : IMember
	{
		private readonly FieldInfo aField;
		private readonly NamingConventions aConventions;

		public FieldMember(IMember parent, Symbol symbol, FieldInfo field, NamingConventions conventions)
		{
			this.Parent = parent;

			this.Symbol = symbol;

			this.aField = field;

			this.aConventions = conventions;
		}

		#region Implementation of IMember

		public IMember Parent { get; }

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

		public Symbol Symbol { get; }

		public Type Type
			=> this.aField.FieldType;

		public Type DeclaringType
			=> this.aField.DeclaringType;

		public MemberInfo Setter
			=> this.aField;

		public string Name
			=> this.aField.Name;

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
			=> this.aField;

		public bool CanPairWith(IMember other)
			=> this.aConventions.CanPair(this, other);

		public Expression CreateGetterExpression(ParameterExpression parentVariable)
			=> Expression.Field(parentVariable, this.aField);

		public Expression CreateSetterExpression(ParameterExpression parentVariable, Expression value)
			=> Expression.Assign(Expression.Field(parentVariable, this.aField), value);

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
			return this.Equals((FieldMember) obj);
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
