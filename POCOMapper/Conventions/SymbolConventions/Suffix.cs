using KST.POCOMapper.Members;

namespace KST.POCOMapper.Conventions.SymbolConventions
{
	public class Suffix : ISymbolConvention
	{
		private readonly string aSuffix;
		private readonly ISymbolConvention aParser;

		public Suffix(ISymbolConvention parser, string suffix)
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
