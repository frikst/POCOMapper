using System;

namespace KST.POCOMapper.exceptions
{
	public class InvalidMapping : Exception
	{
		public InvalidMapping(string message) : base(message)
		{
		}
	}
}
