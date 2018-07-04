using System;
using System.Linq.Expressions;
using System.Reflection;
using KST.POCOMapper.Conventions;

namespace KST.POCOMapper.Members
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

		public FieldMember(IMember parent, FieldInfo field)
		{
			this.Parent = parent;

			this.Symbol = Symbol.Undefined;

			this.aField = field;

			this.aConventions = null;
		}

		#region Implementation of IMember

		public IMember Parent { get; }

		public Symbol Symbol { get; }

		public Type Type
			=> this.aField.FieldType;

		public Type DeclaringType
			=> this.aField.DeclaringType;

		public string Name
			=> this.aField.Name;

		public bool CanPairWith(IMember other)
			=> this.aConventions?.CanPair(this, other) ?? true;

		public bool Readable
			=> true;

		public bool Writable
			=> !this.aField.IsInitOnly;

		public Expression CreateGetterExpression(ParameterExpression parentVariable)
			=> Expression.Field(parentVariable, this.aField);

		public Expression CreateSetterExpression(ParameterExpression parentVariable, Expression value)
			=> Expression.Assign(Expression.Field(parentVariable, this.aField), value);

		#endregion

		public override string ToString()
		{
			if (this.Parent != null)
				return $"{this.Parent}.[F]{this.Name}";
			return $"[F]{this.Name}";
		}

		public override bool Equals(object obj)
		{
			if (obj is FieldMember fieldObj)
				return fieldObj.aField == this.aField && Equals(fieldObj.Parent, this.Parent);

			return false;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (this.aField.GetHashCode() * 397) ^ (this.Parent?.GetHashCode() ?? 0);
			}
		}
	}
}
