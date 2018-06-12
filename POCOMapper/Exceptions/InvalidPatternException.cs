using System;

namespace KST.POCOMapper.Exceptions
{
	public class InvalidPatternException : Exception
	{
		public InvalidPatternException(string pattern, int index, string message = "Bad pattern format")
			: base(string.Format("{0} at char {2}(...{1})", message, pattern.Substring(index), index))
		{
		}
	}
}
