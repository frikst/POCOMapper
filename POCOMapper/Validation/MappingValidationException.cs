using System;

namespace KST.POCOMapper.Validation
{
	public class MappingValidationException : Exception
	{
		public MappingValidationException(string message)
			: base(message)
		{
			
		}
	}
}