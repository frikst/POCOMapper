using System;

namespace POCOMapper.definition.patterns
{
	public interface IPattern
	{
		bool Matches(Type type);
	}
}
