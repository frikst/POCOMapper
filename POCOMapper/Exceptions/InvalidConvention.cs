using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMapper.exceptions
{
	public class InvalidConvention : Exception
	{
		public InvalidConvention(string message) : base(message)
		{
		}
	}
}
