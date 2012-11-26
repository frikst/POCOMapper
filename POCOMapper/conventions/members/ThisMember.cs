using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using POCOMapper.conventions.symbol;

namespace POCOMapper.conventions.members
{
	public class ThisMember<TClass> : IMember
	{
		public ThisMember()
		{
			
		}

		#region Implementation of IMember

		public IMember Parent
		{
			get { return null; }
		}

		public Symbol Symbol
		{
			get { return new Symbol(new string[] { "this" }); }
		}

		public Type Type
		{
			get { return typeof(TClass); }
		}

		public Type DeclaringType
		{
			get { return typeof(TClass); }
		}

		public MemberInfo Getter
		{
			get { throw new NotImplementedException(); }
		}

		public MemberInfo Setter
		{
			get { throw new NotImplementedException(); }
		}

		public string Name
		{
			get { return "this"; }
		}

		public string FullName
		{
			get { return this.Name; }
		}

		public bool CanPairWith(IMember other)
		{
			return true;
		}

		public Expression CreateGetterExpression(ParameterExpression parentVariable)
		{
			return parentVariable;
		}

		public Expression CreateSetterExpression(ParameterExpression parentVariable, Expression value)
		{
			throw new NotImplementedException();
		}

		#endregion

		public bool Equals(ThisMember<TClass> other)
		{
			return !ReferenceEquals(null, other);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (ThisMember<TClass>)) return false;
			return Equals((ThisMember<TClass>) obj);
		}

		public override int GetHashCode()
		{
			return 0;
		}
	}
}
