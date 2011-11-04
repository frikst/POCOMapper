using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMapper;
using POCOMapperTest;

namespace O2OMapperTest
{
	class Program
	{
		static void Main(string[] args)
		{
			IMapping<int[], HashSet<int>> map = TestMapping.Instance.GetMapping<int[], HashSet<int>>();

			HashSet<int> o = map.Map(new int[] { 1, 2, 3 });


		}
	}
}
