using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMapper.definition.patterns
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
