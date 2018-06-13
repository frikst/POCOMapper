using System;
using System.Collections.Generic;
using System.Linq;

namespace KST.POCOMapper.Conventions.Symbols
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

		public override bool Equals(object other)
		{
			if (other is Symbol otherSymbol)
				return this.aHashCode == otherSymbol.aHashCode && otherSymbol.aParts.SequenceEqual(this.aParts);

			return false;
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

		public override string ToString()
		{
			return string.Join("_", this.aParts);
		}

		public static Symbol operator +(Symbol first, Symbol second)
		{
			return new Symbol(first.aParts.Concat(second.aParts));
		}

		public bool HasPrefix(string prefix)
		{
			return this.aParts[0] == prefix;
		}

		public bool HasPrefix(Symbol prefix)
		{
			if (this.aParts.Length < prefix.aParts.Length)
				return false;

			return this.aParts
				.Take(prefix.aParts.Length)
				.SequenceEqual(prefix.aParts);
		}

		public Symbol GetWithoutPrefix()
			=> new Symbol(this.aParts.Skip(1));

		public Symbol GetWithoutPrefix(Symbol prefix)
			=> new Symbol(this.aParts.Skip(prefix.aParts.Length));
	}
}
