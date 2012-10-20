using System;
using System.Text;

namespace POCOMapper.typePatterns
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

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(this.aItem);
			for (int i = 0; i < this.aDimensionCount; i++)
			{
				sb.Append("[]");
			}

			return sb.ToString();
		}

		#endregion
	}
}
