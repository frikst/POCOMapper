using System;
using System.Linq.Expressions;
using System.Reflection;

namespace KST.POCOMapper.Members
{
	public interface IMember
	{
		IMember Parent { get; }
		int Depth { get; }

		Type DeclaringType { get; }

		Symbol Symbol { get; }
		string Name { get; }
		string FullName { get; }

		Type Type { get; }

		bool Readable { get; }
		bool Writable { get; }

		bool CanPairWith(IMember other);

		Expression CreateGetterExpression(ParameterExpression parentVariable);
		Expression CreateSetterExpression(ParameterExpression parentVariable, Expression value);
	}
}
