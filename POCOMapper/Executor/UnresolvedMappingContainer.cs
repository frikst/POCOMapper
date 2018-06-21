using System;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Executor
{
    public class UnresolvedMappingContainer
    {
	    private readonly MappingContainer aContainer;
	    private readonly TypeMappingDefinitionContainer aDefinitionContainer;

	    internal UnresolvedMappingContainer(MappingContainer container, TypeMappingDefinitionContainer definitionContainer)
	    {
		    this.aContainer = container;
		    this.aDefinitionContainer = definitionContainer;
	    }

	    public bool TryGetUnresolvedMapping(Type from, Type to, out IUnresolvedMapping mapping)
	    {
		    if (this.aDefinitionContainer.ContainsMapping(from, to))
		    {
			    mapping = new UnresolvedMapping(this.aContainer, from, to);
			    return true;
		    }

		    mapping = null;
		    return false;
	    }

	    public IUnresolvedMapping GetUnresolvedMapping(Type from, Type to)
	    {
		    if (this.TryGetUnresolvedMapping(from, to, out var mapping))
			    return mapping;

		    throw new UnknownMappingException(from, to);
	    }

	    public bool TryGetUnresolvedMapping<TFrom, TTo>(out IUnresolvedMapping<TFrom, TTo> mapping)
	    {
		    if (this.aDefinitionContainer.ContainsMapping(typeof(TFrom), typeof(TTo)))
		    {
			    mapping = new UnresolvedMapping<TFrom, TTo>(this.aContainer);
			    return true;
		    }

		    mapping = null;
		    return false;
	    }

	    public IUnresolvedMapping<TFrom, TTo> GetUnresolvedMapping<TFrom, TTo>()
	    {
		    if (this.TryGetUnresolvedMapping<TFrom, TTo>(out var mapping))
			    return mapping;

		    throw new UnknownMappingException(typeof(TFrom), typeof(TTo));
	    }
    }
}
