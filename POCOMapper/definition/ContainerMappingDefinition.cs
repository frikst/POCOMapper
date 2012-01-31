using System;
using POCOMapper.mapping.@base;

namespace POCOMapper.definition
{
	/// <summary>
	/// Container mapping specification definition class.
	/// </summary>
	/// <typeparam name="TFrom">Source collection.</typeparam>
	/// <typeparam name="TTo">Destination collection.</typeparam>
	public class ContainerMappingDefinition<TFrom, TTo> : IMappingDefinition
	{
		private Type aMapping;

		#region Implementation of IMappingDefinition

		IMapping IMappingDefinition.CreateMapping(MappingImplementation allMappings, Type from, Type to)
		{
			Type typedMapping = aMapping.MakeGenericType(from, to);
			return (IMapping)Activator.CreateInstance(typedMapping, allMappings);
		}

		Type IMappingDefinition.From
		{
			get { return typeof(TFrom); }
		}

		Type IMappingDefinition.To
		{
			get { return typeof(TTo); }
		}

		MappingType IMappingDefinition.Type
		{
			get { return MappingType.ContainerMapping; }
		}

		#endregion

		/// <summary>
		/// Mapping class specified by the <typeparamref name="TMapping"/> should be used for
		/// mapping the <typeparamref name="TFrom"/> collection to the <typeparamref name="TTo"/> collection.
		/// </summary>
		/// <typeparam name="TMapping"></typeparam>
		public void Using<TMapping>()
			where TMapping : IMapping<TFrom, TTo>
		{
			this.aMapping = typeof(TMapping).GetGenericTypeDefinition();
		}
	}
}
