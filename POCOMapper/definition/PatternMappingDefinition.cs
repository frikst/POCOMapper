using System;
using POCOMapper.mapping.@base;
using POCOMapper.typePatterns;

namespace POCOMapper.definition
{
	/// <summary>
	/// Pattern mapping specification definition class.
	/// </summary>
	public class PatternMappingDefinition : IMappingDefinition
	{
		private readonly IPattern aPatternFrom;
		private readonly IPattern aPatternTo;

		private Type aMapping;

		internal PatternMappingDefinition(IPattern patternFrom, IPattern patternTo)
		{
			aPatternFrom = patternFrom;
			aPatternTo = patternTo;
			aMapping = null;
		}

		#region Implementation of IMappingDefinition

		IMapping IMappingDefinition.CreateMapping(MappingImplementation allMappings, Type from, Type to)
		{
			Type typedMapping = aMapping.MakeGenericType(from, to);
			return (IMapping)Activator.CreateInstance(typedMapping, allMappings);
		}

		bool IMappingDefinition.IsFrom(Type from)
		{
			return this.aPatternFrom.Matches(from);
		}

		bool IMappingDefinition.IsTo(Type to)
		{
			return this.aPatternTo.Matches(to);
		}

		Tuple<Type, Type> IMappingDefinition.GetKey()
		{
			return null;
		}

		#endregion

		/// <summary>
		/// Mapping class specified by the <typeparamref name="TMapping"/> should be used for
		/// mapping the <typeparamref name="TFrom"/> collection to the <typeparamref name="TTo"/> collection.
		/// </summary>
		/// <typeparam name="TMapping"></typeparam>
		public void Using<TMapping>()
			where TMapping : IMapping
		{
			this.aMapping = typeof(TMapping).GetGenericTypeDefinition();
		}
	}
}
