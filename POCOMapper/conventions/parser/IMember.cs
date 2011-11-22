using System;
using System.Linq.Expressions;
using System.Reflection;
using POCOMapper.conventions.symbol;

namespace POCOMapper.conventions.parser
{
	public interface IMember
	{
		IMember Parent { get; }

		Symbol Symbol { get; }
		Type Type { get; }

		MemberInfo Getter { get; }
		MemberInfo Setter { get; }

		Expression CreateGetterExpression(ParameterExpression parentVariable);
		Expression CreateSetterExpression(ParameterExpression parentVariable, Expression value);
	}
}
