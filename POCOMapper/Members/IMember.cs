﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace KST.POCOMapper.Members
{
	public interface IMember
	{
		IMember Parent { get; }

		int Depth { get; }

		Symbol Symbol { get; }
		Type Type { get; }

		Type DeclaringType { get; }

		MemberInfo Getter { get; }
		MemberInfo Setter { get; }
		string Name { get; }

		string FullName { get; }

		bool CanPairWith(IMember other);

		Expression CreateGetterExpression(ParameterExpression parentVariable);
		Expression CreateSetterExpression(ParameterExpression parentVariable, Expression value);
	}
}