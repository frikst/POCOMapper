using System;

namespace KST.POCOMapper.SpecialRules
{
	public interface IEqualityRules : ISpecialRules
	{
		(Delegate IdFrom, Delegate IdTo) GetIdSelectors();
	}
}
