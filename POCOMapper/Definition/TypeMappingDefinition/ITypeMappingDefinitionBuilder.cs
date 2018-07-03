using KST.POCOMapper.Executor.TypeMappingDefinition;

namespace KST.POCOMapper.Definition.TypeMappingDefinition
{
	internal interface ITypeMappingDefinitionBuilder
	{
		ITypeMappingDefinition Finish();
	}
}
