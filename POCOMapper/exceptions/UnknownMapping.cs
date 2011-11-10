using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMapper.exceptions
{
	public class UnknownMapping : Exception
	{
		public UnknownMapping(Type from, Type to)
		{
			this.From = from;
			this.To = to;
		}

		public Type To { get; private set; }
		public Type From { get; private set; }

		public override string Message
		{
			get
			{
				return string.Format("Cannot convert from {0} to {1}", this.From.FullName, this.To.FullName);
			}
		}
	}
}
