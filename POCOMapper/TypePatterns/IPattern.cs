using System;

namespace POCOMapper.typePatterns
{
	public interface IPattern
	{
		bool Matches(Type type);
		string ToString();
	}
}
