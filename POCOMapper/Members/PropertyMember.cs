using System;
using System.Linq.Expressions;
using System.Reflection;
using KST.POCOMapper.Conventions;

namespace KST.POCOMapper.Members
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

		public PropertyMember(IMember parent, PropertyInfo property)
		{
			this.Parent = parent;

			this.Symbol = Symbol.Undefined;

			this.aProperty = property;

			this.aConventions = null;
		}

		public PropertyInfo Property
			=> this.aProperty;

		#region Implementation of IMember

		public IMember Parent { get; }

		public Symbol Symbol { get; }

		public Type Type
			=> this.aProperty.PropertyType;

		public Type DeclaringType
			=> this.aProperty.DeclaringType;

		public string Name
			=> this.aProperty.Name;

		public bool Readable
			=> this.aProperty.CanRead;

		public bool Writable
			=> this.aProperty.CanWrite;

		public bool CanPairWith(IMember other)
			=> this.aConventions?.CanPair(this, other) ?? true;

		public Expression CreateGetterExpression(ParameterExpression parentVariable)
		{
			if (this.aProperty.CanRead)
				return Expression.Property(parentVariable, this.aProperty);
			return null;
		}

		public Expression CreateSetterExpression(ParameterExpression parentVariable, Expression value)
		{
			if (this.aProperty.CanWrite)
				return Expression.Assign(Expression.Property(parentVariable, this.aProperty), value);
			return null;
		}

		#endregion

		public override string ToString()
		{
			if (this.Parent != null)
				return $"{this.Parent}.[P]{this.Name}";
			return $"[P]{this.Name}";
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
				return (this.aProperty.GetHashCode() * 397) ^ (this.Parent?.GetHashCode() ?? 0);
			}
		}
	}
}
