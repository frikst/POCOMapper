using System;
using KST.POCOMapper.SpecialRules;

namespace KST.POCOMapper.Executor
{
    public class SpecialRulesContainer
    {
	    private readonly TypeMappingDefinitionContainer aDefinitionContainer;

	    internal SpecialRulesContainer(TypeMappingDefinitionContainer definitionContainer)
		{
			this.aDefinitionContainer = definitionContainer;
		}

	    public TRules GetRules<TRules>(Type from, Type to)
			where TRules : class, ISpecialRules
	    {
		    return this.aDefinitionContainer.GetSpecialRules<TRules>(from, to);
	    }

	    public TRules GetRules<TFrom, TTo, TRules>()
			where TRules : class, ISpecialRules
	    {
		    return this.aDefinitionContainer.GetSpecialRules<TRules>(typeof(TFrom), typeof(TTo));
	    }
    }
}
