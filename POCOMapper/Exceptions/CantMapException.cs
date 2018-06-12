using System;

namespace KST.POCOMapper.Exceptions
{
	public class CantMapException : Exception
	{
		public CantMapException(string message) : base(message)
		{
		}

		public CantMapException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public CantMapException()
		{
		}
	}
}
