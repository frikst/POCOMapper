using POCOMapper.conventions;
using POCOMapper.definition;

namespace POCOMapperTest
{
	public class TestMapping : MappingDefinition
	{
		private TestMapping()
		{
			CreateMap<Test1, Test2>();
			CreateMap<Test1Child, Test2Child>();

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
