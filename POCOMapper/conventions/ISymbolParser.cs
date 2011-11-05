using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMapper.conventions
{
	public interface ISymbolParser
	{
		Symbol Parse(string symbol);
	}
}
