using System;
using System.Collections;
using System.Collections.Generic;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Collection
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
				throw new InvalidMappingException($"Collection item of {typeof(TFrom).Name} is not {typeof(TItemFrom).Name}");
			if (!typeof(IEnumerable<>).MakeGenericType(typeof(TItemTo)).IsAssignableFrom(typeof(TTo)))
				throw new InvalidMappingException($"Collection item of {typeof(TTo).Name} is not {typeof(TItemTo).Name}");

			this.CfgSelectIdFrom = selectIdFrom;
			this.CfgSelectIdTo = selectIdTo;

			return this;
		}

		internal Delegate CfgSelectIdFrom { get; private set; }
		internal Delegate CfgSelectIdTo { get; private set; }
		internal CollectionMappingType CfgType { get; private set; }

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingDefinitionInformation mappingDefinition)
		{
			switch (this.CfgType)
			{
				case CollectionMappingType.List:
					return new EnumerableToList<TFrom, TTo>(mappingDefinition);
				case CollectionMappingType.Array:
					return new EnumerableToArray<TFrom, TTo>(mappingDefinition, this.CfgSelectIdFrom, this.CfgSelectIdTo);
				default:
					return new EnumerableToEnumerable<TFrom, TTo>(mappingDefinition);
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

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingDefinitionInformation mappingDefinition)
		{
			if (this.CfgSelectIdFrom != null && this.CfgSelectIdTo != null)
			{
				Type itemFrom = this.CfgSelectIdFrom.Method.GetParameters()[0].ParameterType;
				Type itemTo = this.CfgSelectIdTo.Method.GetParameters()[0].ParameterType;

				if (!typeof(IEnumerable<>).MakeGenericType(itemFrom).IsAssignableFrom(typeof(TFrom)))
					throw new InvalidMappingException($"Collection item of {typeof(TFrom).Name} is not {itemFrom.Name}");
				if (!typeof(IEnumerable<>).MakeGenericType(itemTo).IsAssignableFrom(typeof(TTo)))
					throw new InvalidMappingException($"Collection item of {typeof(TTo).Name} is not {itemTo.Name}");
			}

			switch (this.CfgType)
			{
				case CollectionMappingType.List:
					return new EnumerableToList<TFrom, TTo>(mappingDefinition);
				case CollectionMappingType.Array:
					return new EnumerableToArray<TFrom, TTo>(mappingDefinition, this.CfgSelectIdFrom, this.CfgSelectIdTo);
				default:
					return new EnumerableToEnumerable<TFrom, TTo>(mappingDefinition);
			}
		}

		#endregion
	}
}
