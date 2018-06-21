using System;
using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Conventions;
using KST.POCOMapper.Definition.ChildProcessingDefinition;
using KST.POCOMapper.Definition.TypeMappingDefinition;

namespace KST.POCOMapper.Executor
{
    public class MappingDefinitionInformation
    {
	    private readonly List<IChildAssociationPostprocessing> aChildPostprocessings;

	    internal MappingDefinitionInformation(IEnumerable<ITypeMappingDefinition> mappingDefinitions, IEnumerable<IChildAssociationPostprocessing> childPostprocessings, NamingConventions fromConventions, NamingConventions toConventions)
	    {
		    var mappingDefinitionContainer = new TypeMappingDefinitionContainer(this, mappingDefinitions);

		    this.Mappings = new MappingContainer(mappingDefinitionContainer);
		    this.UnresolvedMappings = new UnresolvedMappingContainer(this.Mappings, mappingDefinitionContainer);

		    this.aChildPostprocessings = childPostprocessings.ToList();

		    this.FromConventions = fromConventions;
		    this.ToConventions = toConventions;
	    }

	    public MappingContainer Mappings { get; }

	    public UnresolvedMappingContainer UnresolvedMappings { get; }

	    public NamingConventions FromConventions { get; }
	    public NamingConventions ToConventions { get; }

	    public Delegate GetChildPostprocessing(Type parent, Type child)
	    {
		    foreach (IChildAssociationPostprocessing item in this.aChildPostprocessings)
		    {
			    if (item.Parent.IsAssignableFrom(parent) && item.Child.IsAssignableFrom(child))
				    return item.PostprocessDelegate;
		    }

		    return null;
	    }
    }
}
