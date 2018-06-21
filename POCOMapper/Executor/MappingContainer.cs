using System;
using System.Collections.Generic;
using KST.POCOMapper.Definition.TypeMappingDefinition;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Executor
{
    public class MappingContainer
    {
	    private readonly TypeMappingDefinitionContainer aDefinitionContainer;
	    private readonly Dictionary<TypePair, IMapping> aMappings;

	    internal MappingContainer(TypeMappingDefinitionContainer definitionContainer)
	    {
		    this.aDefinitionContainer = definitionContainer;
		    this.aMappings = new Dictionary<TypePair, IMapping>();
	    }

	    public bool TryGetMapping(Type from, Type to, out IMapping mapping)
	    {
		    var typePair = new TypePair(from, to);

		    if (this.aMappings.TryGetValue(typePair, out mapping))
			    return true;

		    if (this.aDefinitionContainer.TryCreateMapping(from, to, out mapping))
			    return true;

		    return false;
	    }

	    /// <summary>
	    /// Finds the mapping from the type specified by the <paramref name="from"/> parameter to the type specified
	    /// by the <paramref name="to"/> parameter.
	    /// </summary>
	    /// <param name="from">Class from the source model.</param>
	    /// <param name="to">Class from the destination model.</param>
	    /// <returns>The mapping specified by the parameters.</returns>
	    public IMapping GetMapping(Type from, Type to)
	    {
		    if (this.TryGetMapping(from, to, out var mapping))
			    return mapping;

		    throw new UnknownMappingException(from, to);
	    }

	    public bool TryGetMapping<TFrom, TTo>(out IMapping<TFrom, TTo> mapping)
	    {
		    if (this.TryGetMapping(typeof(TFrom), typeof(TTo), out var untypedMapping))
		    {
			    mapping = (IMapping<TFrom, TTo>) untypedMapping;
			    return true;
		    }

		    mapping = null;
		    return false;
	    }

	    /// <summary>
	    /// Type-safe version of the GetMapping method. Finds the mapping from the type specified by
	    /// the <typeparamref name="TFrom"/> parameter to the type specified by the <typeparamref name="TTo"/> parameter.
	    /// </summary>
	    /// <typeparam name="TFrom">Class from the source model.</typeparam>
	    /// <typeparam name="TTo">Class from the destination model.</typeparam>
	    /// <returns>The mapping specified by the type parameters.</returns>
	    public IMapping<TFrom, TTo> GetMapping<TFrom, TTo>()
		    => (IMapping<TFrom, TTo>) this.GetMapping(typeof(TFrom), typeof(TTo));

	    public void AcceptForAll(IMappingVisitor visitor)
	    {
		    visitor.Begin();

		    bool first = true;
		    foreach (var typePair in this.aDefinitionContainer.VisitableMappings)
		    {
			    var mapping = this.GetMapping(typePair.From, typePair.To);

			    if (!first)
				    visitor.Next();
			    first = false;

			    mapping.Accept(visitor);
		    }

		    visitor.End();
	    }
    }
}
