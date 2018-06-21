using System;
using System.Linq.Expressions;

namespace KST.POCOMapper.Members
{
	public interface IMember
	{
		IMember Parent { get; }

		Type DeclaringType { get; }

		Symbol Symbol { get; }
		string Name { get; }

		Type Type { get; }

		bool Readable { get; }
		bool Writable { get; }

		bool CanPairWith(IMember other);

		Expression CreateGetterExpression(ParameterExpression parentVariable);
		Expression CreateSetterExpression(ParameterExpression parentVariable, Expression value);
	}
}
