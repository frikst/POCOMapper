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

			Test2 o2 = TestMapping.Instance.Map<Test1, Test2>(new Test1());
		}
	}
}
