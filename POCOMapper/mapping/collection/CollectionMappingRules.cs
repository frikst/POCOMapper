using System;
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
			this.CfgSelectIdFrom = null;
			this.CfgSelectIdTo = null;
		}

		public CollectionMappingRules<TFrom, TTo> MapAs(CollectionMappingType type)
		{
			this.CfgType = type;

			return this;
		}

		public CollectionMappingRules<TFrom, TTo> EntityId<TItemFrom, TItemTo, TItemId>(Func<TItemFrom, TItemId> selectIdFrom, Func<TItemTo, TItemId> selectIdTo)
		{
			this.CfgSelectIdFrom = selectIdFrom;
			this.CfgSelectIdTo = selectIdTo;

			return this;
		}

		internal Delegate CfgSelectIdFrom { get; private set; }
		internal Delegate CfgSelectIdTo { get; private set; }
		internal CollectionMappingType CfgType { get; private set; }

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingImplementation mapping)
		{
			switch (this.CfgType)
			{
				case CollectionMappingType.List:
					return new EnumerableToList<TFrom, TTo>(mapping, this.CfgSelectIdFrom, this.CfgSelectIdTo);
				case CollectionMappingType.Array:
					return new EnumerableToArray<TFrom, TTo>(mapping, this.CfgSelectIdFrom, this.CfgSelectIdTo);
				default:
					return new EnumerableToEnumerable<TFrom, TTo>(mapping, this.CfgSelectIdFrom, this.CfgSelectIdTo);
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
					return new EnumerableToList<TFrom, TTo>(mapping, null, null);
				case CollectionMappingType.Array:
					return new EnumerableToArray<TFrom, TTo>(mapping, null, null);
				default:
					return new EnumerableToEnumerable<TFrom, TTo>(mapping, null, null);
			}
		}

		#endregion
	}
}
