using System.Collections.Generic;

namespace POCOMapperTest
{
	class Test2
	{
		public string aVal1;

		public string Val2 { get; set; }

		private List<int> XXX;

		public void SetVal3(List<int> param)
		{
			this.XXX = param;
		}
	}
}
