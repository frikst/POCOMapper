using System;

namespace KST.POCOMapper.Exceptions
{
	public class UnknownMapping : Exception
	{
		public UnknownMapping(Type from, Type to)
		{
			this.From = from;
			this.To = to;
		}

		public Type To { get; }
		public Type From { get; }

		public override string Message
			=> string.Format("Cannot convert from {0} to {1}", this.From.FullName, this.To.FullName);
	}
}
