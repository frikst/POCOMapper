using POCOMapper;
using POCOMapper.conventions;

namespace POCOMapperTest
{
	public class TestMapping : MappingDefinition
	{
		private TestMapping()
		{
			CreateMap<Test1, Test2>();

			FromConventions
				.SetAttributeConvention(new Prefix("a", new BigCammelCase()));

			ToConventions
				.SetAttributeConvention(new Prefix("a", new BigCammelCase()));
		}

		public static MappingImplementation Instance
		{
			get { return GetInstance<TestMapping>(); }
		}
	}
}
