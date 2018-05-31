using System;

namespace POCOMapper.typePatterns
{
	internal class AnyPattern : IPattern
	{
		#region Implementation of IPattern

		public bool Matches(Type type)
		{
			return true;
		}

		public override string ToString()
		{
			return "?";
		}

		#endregion
	}
}
