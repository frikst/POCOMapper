using System;
using System.Linq.Expressions;
using System.Reflection;
using KST.POCOMapper.Conventions.Symbols;

namespace KST.POCOMapper.Conventions.Members
{
	public class PropertyMember : IMember
	{
		private readonly PropertyInfo aProperty;
		private readonly NamingConventions aConventions;

		public PropertyMember(IMember parent, Symbol symbol, PropertyInfo property, NamingConventions conventions)
		{
			this.Parent = parent;

			this.Symbol = symbol;

			this.aProperty = property;

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
			=> this.aProperty.PropertyType;

		public Type DeclaringType
			=> this.aProperty.DeclaringType;

		public MemberInfo Getter
		{
			get
			{
				if (this.aProperty.CanRead)
					return this.aProperty;
				else
					return null;
			}
		}

		public MemberInfo Setter
		{
			get
			{
				if (this.aProperty.CanWrite)
					return this.aProperty;
				else
					return null;
			}
		}

		public string Name
			=> this.aProperty.Name;

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
			=> this.aConventions.CanPair(this, other);

		public Expression CreateGetterExpression(ParameterExpression parentVariable)
		{
			if (this.Getter != null)
				return Expression.Property(parentVariable, this.aProperty);
			return null;
		}

		public Expression CreateSetterExpression(ParameterExpression parentVariable, Expression value)
		{
			if (this.Setter != null)
				return Expression.Assign(Expression.Property(parentVariable, this.aProperty), value);
			return null;
		}

		#endregion

		public override string ToString()
		{
			if (this.Parent != null)
				return $"{this.Parent}.[P]{this.Symbol}";
			return $"[P]{this.Symbol}";
		}

		public override bool Equals(object obj)
		{
			if (obj is PropertyMember propertyObj)
				return propertyObj.aProperty == this.aProperty && Equals(propertyObj.Parent, this.Parent);

			return false;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((this.aProperty != null ? this.aProperty.GetHashCode() : 0)*397) ^ (this.Parent != null ? this.Parent.GetHashCode() : 0);
			}
		}
	}
}
