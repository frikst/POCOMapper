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
			Test1 t = new Test1();

			for (int i = 0; i < 1000000; i++)
				mapping2.Map(t);

			Console.WriteLine((DateTime.Now - x).TotalMilliseconds);
			x = DateTime.Now;

			for (int i = 0; i < 1000000; i++)
			{
				Test2 ret = new Test2 { aVal1 = t.Val1, Val2 = t.GetVal2() };
				ret.SetVal3(t.aVal3.Select(obj => obj == null ? null : new Test2Child { Name = obj.aName }).ToList());
			}

			Console.WriteLine((DateTime.Now - x).TotalMilliseconds);

			int[] a = new int[1000];
			x = DateTime.Now;

			for (int i = 0; i < 100000; i++)
			{
				a.ToArray();
			}

			Console.WriteLine((DateTime.Now - x).TotalMilliseconds);

			x = DateTime.Now;

			for (int i = 0; i < 100000; i++)
			{
				a.Select(obj=>obj).ToArray();
			}

			Console.WriteLine((DateTime.Now - x).TotalMilliseconds);

			Console.ReadLine();
		}
	}
}
