using System;

namespace KST.POCOMapper.Exceptions
{
	public class UnknownMappingException : Exception
	{
		public UnknownMappingException(Type from, Type to)
			: base($"Cannot convert from {from.FullName} to {to.FullName}")
		{
			this.From = from;
			this.To = to;
		}

		public Type To { get; }
		public Type From { get; }
	}
}
