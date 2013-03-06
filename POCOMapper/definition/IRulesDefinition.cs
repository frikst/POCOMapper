using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMapper.mapping.@base;

namespace POCOMapper.definition
{
	public interface IRulesDefinition
	{
		TRules Rules<TRules>() where TRules : class, IMappingRules, new();
	}

	public interface IRulesDefinition<TFrom, TTo>
	{
		TRules Rules<TRules>() where TRules : class, IMappingRules<TFrom, TTo>, new();
	}
}
