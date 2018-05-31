using System.Text.RegularExpressions;

namespace KST.POCOMapper.conventions.symbol
{
	public class BigCammelCase : ISymbolParser
	{
		#region Implementation of ISymbolParser

		public Symbol Parse(string symbol)
		{
			return new Symbol(Regex.Replace(symbol, "([A-Z])", " $1", RegexOptions.Compiled).Trim().Split(' '));
		}

		#endregion
	}
}
