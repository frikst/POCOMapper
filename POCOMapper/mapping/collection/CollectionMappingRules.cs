using System.Collections;
using POCOMapper.definition;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.collection
{
	public class CollectionMappingRules<TFrom, TTo> : IMappingRules<TFrom, TTo>
		where TFrom : IEnumerable
		where TTo : IEnumerable
	{
		public CollectionMappingRules()
		{
			this.CfgType = CollectionMappingType.Enumerable;
		}

		public CollectionMappingRules<TFrom, TTo> MapAs(CollectionMappingType type)
		{
			this.CfgType = type;

			return this;
		}

		internal CollectionMappingType CfgType { get; private set; }

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingImplementation mapping)
		{
			switch (this.CfgType)
			{
				case CollectionMappingType.List:
					return new EnumerableToList<TFrom, TTo>(mapping);
				case CollectionMappingType.Array:
					return new EnumerableToArray<TFrom, TTo>(mapping);
				default:
					return new EnumerableToEnumerable<TFrom, TTo>(mapping);
			}
		}

		#endregion
	}

	public class CollectionMappingRules : IMappingRules
	{
		public CollectionMappingRules()
		{
			this.CfgType = CollectionMappingType.Enumerable;
		}

		public CollectionMappingRules MapAs(CollectionMappingType type)
		{
			this.CfgType = type;

			return this;
		}

		protected CollectionMappingType CfgType { get; private set; }

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingImplementation mapping)
		{
			switch (this.CfgType)
			{
				case CollectionMappingType.List:
					return new EnumerableToList<TFrom, TTo>(mapping);
				case CollectionMappingType.Array:
					return new EnumerableToArray<TFrom, TTo>(mapping);
				default:
					return new EnumerableToEnumerable<TFrom, TTo>(mapping);
			}
		}

		#endregion
	}
}
