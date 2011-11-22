using System;
using POCOMapper.mapping.@base;

namespace POCOMapper.definition
{
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

		public void Using<TMapping>()
			where TMapping : IMapping<TFrom, TTo>
		{
			this.aMapping = typeof(TMapping).GetGenericTypeDefinition();
		}
	}
}
