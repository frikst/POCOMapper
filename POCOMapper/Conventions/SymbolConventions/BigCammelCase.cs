using KST.POCOMapper.Internal;
using KST.POCOMapper.Members;

namespace KST.POCOMapper.Conventions.SymbolConventions
{
	public class BigCammelCase : ISymbolConvention
	{
		#region Implementation of ISymbolParser

		public Symbol Parse(string symbol)
		{
			return new Symbol(new CammelCaseSplitter(symbol));
		}

		#endregion
	}
}
