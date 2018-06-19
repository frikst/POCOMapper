using System;
using System.Linq.Expressions;
using System.Reflection;

namespace KST.POCOMapper.Members
{
	public class ThisMember<TClass> : IMember
	{
		public ThisMember()
		{
			
		}

		#region Implementation of IMember

		public IMember Parent
			=> null;

		public int Depth
			=> 0;

		public Symbol Symbol
			=> new Symbol(new string[] { "this" });

		public Type Type
			=> typeof(TClass);

		public Type DeclaringType
			=> typeof(TClass);

		public string Name
			=> "this";

		public bool Readable
			=> true;

		public bool Writable
			=> false;

		public bool CanPairWith(IMember other)
			=> true;

		public Expression CreateGetterExpression(ParameterExpression parentVariable)
		{
			return parentVariable;
		}

		public Expression CreateSetterExpression(ParameterExpression parentVariable, Expression value)
			=> throw new NotImplementedException();

		#endregion

		public override bool Equals(object obj)
		{
			return obj is ThisMember<TClass>;
		}

		public override int GetHashCode()
		{
			return 0;
		}
	}
}
