using System;

namespace KST.POCOMapper.Exceptions
{
	public class UnknownMappingException : Exception
	{
		public UnknownMappingException(Type from, Type to)
			: base(string.Format("Cannot convert from {0} to {1}", from.FullName, to.FullName))
		{
			this.From = from;
			this.To = to;
		}

		public Type To { get; }
		public Type From { get; }
	}
}
