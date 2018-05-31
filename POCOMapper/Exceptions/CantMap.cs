using System;

namespace KST.POCOMapper.exceptions
{
	public class CantMap : Exception
	{
		public CantMap(string message) : base(message)
		{
		}

		public CantMap(string message, Exception innerException) : base(message, innerException)
		{
		}

		public CantMap()
		{
		}
	}
}
