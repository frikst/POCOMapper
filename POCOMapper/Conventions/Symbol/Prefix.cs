namespace KST.POCOMapper.conventions.symbol
{
	public class Prefix : ISymbolParser
	{
		private readonly string aPreffix;
		private readonly ISymbolParser aParser;

		public Prefix(string preffix, ISymbolParser parser)
		{
			this.aParser = parser;
			this.aPreffix = preffix;
		}

		#region Implementation of ISymbolParser

		public Symbol Parse(string symbol)
		{
			if (symbol.StartsWith(this.aPreffix))
				symbol = symbol.Substring(this.aPreffix.Length);

			return this.aParser.Parse(symbol);
		}

		#endregion
	}
}
