using System;
using System.Collections;
using System.Collections.Generic;
using POCOMapper.definition;
using POCOMapper.exceptions;
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
			if (!typeof(IEnumerable<>).MakeGenericType(typeof(TItemFrom)).IsAssignableFrom(typeof(TFrom)))
				throw new InvalidMapping(string.Format("Collection item of {0} is not {1}", typeof(TItemFrom).Name, typeof(TFrom).Name));
			if (!typeof(IEnumerable<>).MakeGenericType(typeof(TItemTo)).IsAssignableFrom(typeof(TTo)))
				throw new InvalidMapping(string.Format("Collection item of {0} is not {1}", typeof(TItemTo).Name, typeof(TTo).Name));

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
			this.CfgSelectIdFrom = null;
			this.CfgSelectIdTo = null;
		}

		public CollectionMappingRules MapAs(CollectionMappingType type)
		{
			this.CfgType = type;

			return this;
		}

		public CollectionMappingRules EntityId<TItemFrom, TItemTo, TItemId>(Func<TItemFrom, TItemId> selectIdFrom, Func<TItemTo, TItemId> selectIdTo)
		{
			this.CfgSelectIdFrom = selectIdFrom;
			this.CfgSelectIdTo = selectIdTo;

			return this;
		}

		internal Delegate CfgSelectIdFrom { get; private set; }
		internal Delegate CfgSelectIdTo { get; private set; }
		internal CollectionMappingType CfgType { get; private set; }

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingImplementation mapping)
		{
			if (this.CfgSelectIdFrom != null && this.CfgSelectIdTo != null)
			{
				Type itemFrom = this.CfgSelectIdFrom.Method.GetParameters()[0].ParameterType;
				Type itemTo = this.CfgSelectIdFrom.Method.GetParameters()[0].ParameterType;

				if (!typeof(IEnumerable<>).MakeGenericType(itemFrom).IsAssignableFrom(typeof(TFrom)))
					throw new InvalidMapping(string.Format("Collection item of {0} is not {1}", itemFrom.Name, typeof(TFrom).Name));
				if (!typeof(IEnumerable<>).MakeGenericType(itemTo).IsAssignableFrom(typeof(TTo)))
					throw new InvalidMapping(string.Format("Collection item of {0} is not {1}", itemTo.Name, typeof(TTo).Name));
			}

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
}
