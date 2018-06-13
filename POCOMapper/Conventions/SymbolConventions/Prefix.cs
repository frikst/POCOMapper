using KST.POCOMapper.Members;

namespace KST.POCOMapper.Conventions.SymbolConventions
{
	public class Prefix : ISymbolConvention
	{
		private readonly string aPreffix;
		private readonly ISymbolConvention aParser;

		public Prefix(string preffix, ISymbolConvention parser)
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
