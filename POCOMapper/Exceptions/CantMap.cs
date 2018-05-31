using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMapper.exceptions
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
