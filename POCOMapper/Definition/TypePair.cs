using System;
using System.Collections.Generic;
using System.Text;

namespace KST.POCOMapper.Definition
{
	internal class TypePair
	{
		private readonly object aFrom;
		private readonly object aTo;
		private readonly int aHash;

		public TypePair(Type from, Type to)
		{
			this.aFrom = from;
			this.aTo = to;

			this.aHash = (from.GetHashCode() * 397) ^ to.GetHashCode();
		}

		#region Equality members

		public override bool Equals(object obj)
		{
			if (this == obj)
				return true;

			if (obj is TypePair other)
				return this.aFrom == other.aFrom && this.aTo == other.aTo;

			return false;
		}

		public override int GetHashCode()
		{
			return this.aHash;
		}

		#endregion
	}
}
