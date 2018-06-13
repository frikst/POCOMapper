using KST.POCOMapper.Members;

namespace KST.POCOMapper.Conventions.SymbolConventions
{
	public interface ISymbolConvention
	{
		Symbol Parse(string symbol);
	}
}
