using System;

namespace KST.POCOMapper.Executor
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

		public Type From
			=> (Type) this.aFrom;

		public Type To
			=> (Type) this.aTo;

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
