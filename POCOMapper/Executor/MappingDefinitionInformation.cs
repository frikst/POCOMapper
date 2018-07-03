using System;
using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Conventions;
using KST.POCOMapper.Definition.ChildProcessingDefinition;
using KST.POCOMapper.Executor.TypeMappingDefinition;

namespace KST.POCOMapper.Executor
{
    public class MappingDefinitionInformation
    {
	    private readonly List<IChildAssociationPostprocessing> aChildPostprocessings;
	    private readonly TypeMappingDefinitionContainer aTypeMappingDefinitionContainer;

	    internal MappingDefinitionInformation(IEnumerable<ITypeMappingDefinition> mappingDefinitions, IEnumerable<IChildAssociationPostprocessing> childPostprocessings, NamingConventions fromConventions, NamingConventions toConventions)
	    {
		    this.aTypeMappingDefinitionContainer = new TypeMappingDefinitionContainer(this, mappingDefinitions);

		    this.Mappings = new MappingContainer(this.aTypeMappingDefinitionContainer);
		    this.UnresolvedMappings = new UnresolvedMappingContainer(this.Mappings, this.aTypeMappingDefinitionContainer);

		    this.aChildPostprocessings = childPostprocessings.ToList();

		    this.FromConventions = fromConventions;
		    this.ToConventions = toConventions;

			this.FromConventions.SetMappingDefinition(this);
		    this.ToConventions.SetMappingDefinition(this);
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

	    public bool HasMapping(Type from, Type to)
		    => this.aTypeMappingDefinitionContainer.ContainsMapping(@from, to);

	    public bool HasMapping<TFrom, TTo>()
		    => this.HasMapping(typeof(TFrom), typeof(TTo));
    }
}
