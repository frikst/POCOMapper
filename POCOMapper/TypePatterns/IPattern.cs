using System;

namespace KST.POCOMapper.typePatterns
{
	public interface IPattern
	{
		bool Matches(Type type);
		string ToString();
	}
}
