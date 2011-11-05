namespace POCOMapper.conventions
{
	public class Suffix : ISymbolParser
	{
		private readonly string aSuffix;
		private readonly ISymbolParser aParser;

		public Suffix(ISymbolParser parser, string suffix)
		{
			this.aParser = parser;
			this.aSuffix = suffix;
		}

		#region Implementation of ISymbolParser

		public Symbol Parse(string symbol)
		{
			if (symbol.EndsWith(this.aSuffix))
				symbol = symbol.Substring(0, symbol.Length - this.aSuffix.Length);

			return this.aParser.Parse(symbol);
		}

		#endregion
	}
}
