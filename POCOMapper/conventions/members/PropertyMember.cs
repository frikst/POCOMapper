using System;
using System.Linq.Expressions;
using System.Reflection;
using POCOMapper.conventions.symbol;

namespace POCOMapper.conventions.members
{
	public class PropertyMember : IMember
	{
		private readonly PropertyInfo aProperty;
		private readonly Conventions aConventions;

		public PropertyMember(IMember parent, Symbol symbol, PropertyInfo property, Conventions conventions)
		{
			this.Parent = parent;

			this.Symbol = symbol;

			this.aProperty = property;

			this.aConventions = conventions;
		}

		#region Implementation of IMember

		public IMember Parent { get; private set; }

		public Symbol Symbol { get; private set; }

		public Type Type
		{
			get { return this.aProperty.PropertyType; }
		}

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
		{
			get { return this.aProperty.Name; }
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
				return string.Format("{0}.[P]{1}", this.Parent, this.Symbol);
			return string.Format("[P]{0}", this.Symbol);
		}
	}
}
