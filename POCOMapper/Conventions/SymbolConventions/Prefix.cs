using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Members;

namespace KST.POCOMapper.Conventions.SymbolConventions
{
	public class Prefix : ISymbolConvention
	{
		private readonly string[] aPreffixes;
		private readonly ISymbolConvention aParser;

		public Prefix(string preffix, ISymbolConvention parser)
            : this(new []{preffix}, parser)
		{
		}

		public Prefix(IEnumerable<string> preffixes, ISymbolConvention parser)
		{
			this.aParser = parser;
			this.aPreffixes = preffixes.ToArray();
		}

		#region Implementation of ISymbolParser

		public Symbol Parse(string symbol)
		{
            foreach (var preffix in this.aPreffixes)
                if (symbol.StartsWith(preffix))
                    symbol = symbol.Substring(preffix.Length);

            return this.aParser.Parse(symbol);
		}

		#endregion
	}
}
