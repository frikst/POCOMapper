using System;
using System.Linq.Expressions;
using System.Reflection;
using POCOMapper.conventions.symbol;

namespace POCOMapper.conventions.members
{
	public interface IMember
	{
		IMember Parent { get; }

		Symbol Symbol { get; }
		Type Type { get; }

		MemberInfo Getter { get; }
		MemberInfo Setter { get; }
		string Name { get; }

		string FullName { get; }

		bool CanPairWith(IMember other);

		Expression CreateGetterExpression(ParameterExpression parentVariable);
		Expression CreateSetterExpression(ParameterExpression parentVariable, Expression value);
	}
}
