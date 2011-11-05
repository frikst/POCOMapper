using System.Collections.Generic;
using System.Linq;

namespace POCOMapper.conventions
{
	public struct Symbol
	{
		private readonly string[] aParts;
		private readonly int aHashCode;

		public Symbol(IEnumerable<string> parts)
		{
			this.aParts = parts.Select(x => x.ToLower()).ToArray();
			this.aHashCode = (int)this.aParts.Sum(x => (long)x.GetHashCode());
		}

		public bool Equals(Symbol other)
		{
			return this.aHashCode == other.aHashCode && other.aParts.SequenceEqual(this.aParts);
		}

		public override bool Equals(object other)
		{
			if (ReferenceEquals(null, other))
				return false;

			if (!(other is Symbol))
				return false;

			return Equals((Symbol) other);
		}

		public override int GetHashCode()
		{
			return this.aHashCode;
		}

		public static bool operator ==(Symbol first, Symbol other)
		{
			return Equals(first, other);
		}

		public static bool operator !=(Symbol first, Symbol other)
		{
			return !Equals(first, other);
		}

		public bool HasPrefix(string prefix)
		{
			return this.aParts[0] == prefix;
		}

		public Symbol GetWithoutPrefix()
		{
			return new Symbol(aParts.Skip(1));
		}
	}
}
