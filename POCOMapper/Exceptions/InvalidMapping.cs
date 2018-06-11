using System;

namespace KST.POCOMapper.Exceptions
{
	public class InvalidMapping : Exception
	{
		public InvalidMapping(string message) : base(message)
		{
		}
	}
}
