using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Internal.ReflectionMembers;

namespace KST.POCOMapper.Mapping.Base
{
	public static class MappingExtension
	{
		#region Support classes

		private class UnresolvedMapping : IUnresolvedMapping
		{
			public UnresolvedMapping(IMapping resolvedMapping)
			{
				this.ResolvedMapping = resolvedMapping;
			}

			public IMapping ResolvedMapping { get; }
		}

		private class UnresolvedMapping<TFrom, TTo> : IUnresolvedMapping<TFrom, TTo>
		{
			public UnresolvedMapping(IMapping<TFrom, TTo> resolvedMapping)
			{
				this.ResolvedMapping = resolvedMapping;
			}

			public IMapping<TFrom, TTo> ResolvedMapping { get; }

			IMapping IUnresolvedMapping.ResolvedMapping
				=> this.ResolvedMapping;
		}

		#endregion

		private static readonly Dictionary<(Type from, Type to), Func<IMappingRules, MappingDefinitionInformation, IMapping>> aMappingFactoryMethod
			= new Dictionary<(Type from, Type to), Func<IMappingRules, MappingDefinitionInformation, IMapping>>();

		public static IUnresolvedMapping AsUnresolved(this IMapping mapping)
			=> new UnresolvedMapping(mapping);

		public static IUnresolvedMapping<TFrom, TTo> AsUnresolved<TFrom, TTo>(this IMapping<TFrom, TTo> mapping)
			=> new UnresolvedMapping<TFrom, TTo>(mapping);

		public static IMapping Create(this IMappingRules mappingRules, Type from, Type to, MappingDefinitionInformation mappingDefinition)
		{
			Func<IMappingRules, MappingDefinitionInformation, IMapping> factoryFunction;

			if (!MappingExtension.aMappingFactoryMethod.TryGetValue((from, to), out factoryFunction))
			{
				var mappingCreateMethod = MappingRulesMethods.GetCreate(from, to);

				var mappingRulesParameter = Expression.Parameter(typeof(IMappingRules), "mappingRules");
				var mappingDefinitionParameter = Expression.Parameter(typeof(MappingDefinitionInformation), "mappingDefinition");

				var factoryFunctionExpression = Expression.Lambda<Func<IMappingRules, MappingDefinitionInformation, IMapping>>(
					Expression.Call(mappingRulesParameter, mappingCreateMethod, mappingDefinitionParameter),
					mappingRulesParameter,
					mappingDefinitionParameter
				);

				factoryFunction = factoryFunctionExpression.Compile();
				MappingExtension.aMappingFactoryMethod[(from, to)] = factoryFunction;
			}

			return factoryFunction(mappingRules, mappingDefinition);
		}
	}
}
