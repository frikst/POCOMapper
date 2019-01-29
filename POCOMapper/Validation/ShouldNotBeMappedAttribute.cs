using System;

namespace KST.POCOMapper.Validation
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property)]
	public class ShouldNotBeMappedAttribute : Attribute
	{
	}
}
