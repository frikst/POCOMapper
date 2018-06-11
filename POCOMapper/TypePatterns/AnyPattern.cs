using System;

namespace KST.POCOMapper.TypePatterns
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
