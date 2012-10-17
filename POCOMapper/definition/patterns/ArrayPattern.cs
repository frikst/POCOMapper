using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMapper.definition.patterns
{
	internal class ArrayPattern : IPattern
	{
		private IPattern aItem;
		private int aDimensionCount;

		public ArrayPattern(IPattern item, int dimensionCount)
		{
			this.aItem = item;
			this.aDimensionCount = dimensionCount;
		}

		#region Implementation of IPattern

		public bool Matches(Type type)
		{
			if (! type.IsArray)
				return false;

			if (! this.aItem.Matches(type.GetElementType()))
				return false;

			if (type.GetArrayRank() != this.aDimensionCount)
				return false;

			return true;
		}

		#endregion
	}
}
