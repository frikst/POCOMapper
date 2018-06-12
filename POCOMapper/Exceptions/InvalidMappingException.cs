using System;

namespace KST.POCOMapper.Exceptions
{
	public class InvalidMappingException : Exception
	{
		public InvalidMappingException(string message) : base(message)
		{
		}
	}
}
