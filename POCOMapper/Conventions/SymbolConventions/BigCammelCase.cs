using System.Text.RegularExpressions;
using KST.POCOMapper.Members;

namespace KST.POCOMapper.Conventions.SymbolConventions
{
	public class BigCammelCase : ISymbolConvention
	{
		#region Implementation of ISymbolParser

		public Symbol Parse(string symbol)
		{
			return new Symbol(Regex.Replace(symbol, "([A-Z])", " $1", RegexOptions.Compiled).Trim().Split(' '));
		}

		#endregion
	}
}
