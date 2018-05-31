﻿using System;

namespace KST.POCOMapper.exceptions
{
	public class InvalidPattern : Exception
	{
		public InvalidPattern(string pattern, int index)
			: this(pattern, index, "Bad pattern format")
		{
		}

		public InvalidPattern(string pattern, int index, string message)
			: base(string.Format("{0} at char {2}(...{1})", message, pattern.Substring(index), index))
		{
		}
	}
}