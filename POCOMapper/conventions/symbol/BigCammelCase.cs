using System.Text.RegularExpressions;

namespace POCOMapper.conventions.symbol
{
	public class BigCammelCase : ISymbolParser
	{
		#region Implementation of ISymbolParser

		public Symbol Parse(string symbol)
		{
			return new Symbol(Regex.Replace(symbol, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim().Split(' '));
		}

		#endregion
	}
}
