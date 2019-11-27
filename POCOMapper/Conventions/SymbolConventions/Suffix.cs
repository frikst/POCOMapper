using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Members;

namespace KST.POCOMapper.Conventions.SymbolConventions
{
	public class Suffix : ISymbolConvention
	{
		private readonly string[] aSuffixes;
		private readonly ISymbolConvention aParser;

		public Suffix(ISymbolConvention parser, string suffix)
            : this(parser, new []{suffix})
		{
		}

		public Suffix(ISymbolConvention parser, IEnumerable<string> suffixes)
		{
			this.aParser = parser;
			this.aSuffixes = suffixes.ToArray();
		}

		#region Implementation of ISymbolParser

		public Symbol Parse(string symbol)
		{
            foreach (var suffix in this.aSuffixes)
                if (symbol.EndsWith(suffix))
                    symbol = symbol.Substring(0, symbol.Length - suffix.Length);

            return this.aParser.Parse(symbol);
		}

		#endregion
	}
}
