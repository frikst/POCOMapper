using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMapper;

namespace POCOMapperTest
{
	public class TestMapping : MappingDefinition
	{
		private TestMapping()
		{
			//CreateMap<Test1, Test2>();
		}

		public static MappingImplementation Instance
		{
			get { return GetInstance<TestMapping>(); }
		}
	}
}
