using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMapper.exceptions
{
	public class InvalidMapping : Exception
	{
		public InvalidMapping(string message) : base(message)
		{
		}
	}
}
