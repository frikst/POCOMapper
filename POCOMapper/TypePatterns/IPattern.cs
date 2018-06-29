using System;

namespace KST.POCOMapper.TypePatterns
{
	public interface IPattern
	{
		bool Matches(Type type, TypeChecker typeChecker);
		string ToString();
	}
}
