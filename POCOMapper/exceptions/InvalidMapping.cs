using System;

namespace POCOMapper.exceptions
{
	public class InvalidMapping : Exception
	{
		public InvalidMapping(string message) : base(message)
		{
		}
	}
}
