using System;
using System.Collections.Generic;
using System.Linq;
using POCOMapper.mapping.@base;

namespace POCOMapperTest
{
	class Program
	{
		static void Main(string[] args)
		{
			IMapping<int[], HashSet<int>> map = TestMapping.Instance.GetMapping<int[], HashSet<int>>();

			HashSet<int> o = map.Map(new int[] { 1, 2, 3 });

			Test2 o2 = TestMapping.Instance.Map<Test1, Test2>(new Test1());

			var mapping2 = TestMapping.Instance.GetMapping<Test1, Test2>();

			var x = DateTime.Now;

			for (int i = 0; i < 1000000; i++)
				mapping2.Map(new Test1());

			Console.WriteLine((DateTime.Now - x).TotalMilliseconds);

			x = DateTime.Now;

			for (int i = 0; i < 1000000; i++)
			{
				Test1 t = new Test1();
				Test2 ret = new Test2 { aVal1 = t.Val1, Val2 = t.GetVal2() };
				ret.SetVal3(t.aVal3.ToList());
			}

			Console.WriteLine((DateTime.Now - x).TotalMilliseconds);

			Console.ReadLine();
		}
	}
}
