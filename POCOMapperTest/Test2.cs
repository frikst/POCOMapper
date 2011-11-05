using System.Collections.Generic;

namespace POCOMapperTest
{
	class Test2
	{
		public string aVal1;

		public string Val2 { get; set; }

		private List<Test2Child> XXX;

		public void SetVal3(List<Test2Child> param)
		{
			this.XXX = param;
		}
	}

	public class Test2Child
	{
		public string Name { get; set; }
	}
}
