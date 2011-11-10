using System;
using System.Collections.Generic;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.common;

namespace POCOMapper.definition
{
	public abstract class SingleMappingDefinition
	{
		internal abstract IMapping CreateMapping(MappingImplementation allMappings);
		internal abstract Type From { get; }
		internal abstract Type To { get; }
	}

	public class SingleMappingDefinition<TFrom, TTo> : SingleMappingDefinition
	{
		private List<Tuple<Type, Type>> aSubClassMaps = new List<Tuple<Type, Type>>();

		#region Overrides of SingleMappingDefinition

		internal override IMapping CreateMapping(MappingImplementation allMappings)
		{
			IMapping<TFrom, TTo> mapping = new ObjectToObject<TFrom, TTo>(allMappings);

			if (aSubClassMaps.Count > 0)
				mapping = new SubClassToObject<TFrom, TTo>(allMappings, aSubClassMaps, mapping);

			return mapping;
		}

		internal override Type From
		{
			get { return typeof(TFrom); }
		}

		internal override Type To
		{
			get { return typeof(TTo); }
		}

		#endregion

		public SingleMappingDefinition<TFrom, TTo> MapSubClass<TSubFrom, TSubTo>()
			where TSubFrom : TFrom
			where TSubTo : TTo
		{
			this.aSubClassMaps.Add(new Tuple<Type, Type>(typeof(TSubFrom), typeof(TSubTo)));

			return this;
		}
	}
}