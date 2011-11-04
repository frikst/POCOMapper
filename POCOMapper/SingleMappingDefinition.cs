using System;

namespace POCOMapper
{
	public abstract class SingleMappingDefinition
	{
		internal abstract IMapping CreateMapping(MappingImplementation allMappings);
		internal abstract Type From { get; }
		internal abstract Type To { get; }
	}

	public class SingleMappingDefinition<TFrom, TTo> : SingleMappingDefinition
	{
		#region Overrides of SingleMappingDefinition

		internal override IMapping CreateMapping(MappingImplementation allMappings)
		{
			return null;
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
	}
}