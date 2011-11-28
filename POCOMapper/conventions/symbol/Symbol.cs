using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOMapper.conventions.symbol
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

		#region Equality members

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

		#endregion

		public bool HasPrefix(string prefix)
		{
			return this.aParts[0] == prefix;
		}

		public bool HasPrefix(Symbol prefix)
		{
			foreach (Tuple<string, string> part in Enumerable.Zip(aParts, prefix.aParts, (a, b) => new Tuple<string, string>(a, b)))
			{
				if (part.Item1 != part.Item2)
					return false;
			}
			return true;
		}

		public Symbol GetWithoutPrefix()
		{
			return new Symbol(aParts.Skip(1));
		}

		public Symbol GetWithoutPrefix(Symbol prefix)
		{
			return new Symbol(aParts.Skip(prefix.aParts.Length));
		}
	}
}
