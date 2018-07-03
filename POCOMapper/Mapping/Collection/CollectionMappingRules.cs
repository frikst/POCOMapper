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
		private Delegate aSelectIdFrom;
		private Delegate aSelectIdTo;
		private CollectionMappingType aMappingType;

		public CollectionMappingRules()
		{
			this.aMappingType = CollectionMappingType.Enumerable;
			this.aSelectIdFrom = null;
			this.aSelectIdTo = null;
		}

		public CollectionMappingRules<TFrom, TTo> MapAs(CollectionMappingType type)
		{
			this.aMappingType = type;

			return this;
		}

		public CollectionMappingRules<TFrom, TTo> EntityId<TItemFrom, TItemTo, TItemId>(Func<TItemFrom, TItemId> selectIdFrom, Func<TItemTo, TItemId> selectIdTo)
		{
			if (!typeof(IEnumerable<>).MakeGenericType(typeof(TItemFrom)).IsAssignableFrom(typeof(TFrom)))
				throw new InvalidMappingException($"Collection item of {typeof(TFrom).Name} is not {typeof(TItemFrom).Name}");
			if (!typeof(IEnumerable<>).MakeGenericType(typeof(TItemTo)).IsAssignableFrom(typeof(TTo)))
				throw new InvalidMappingException($"Collection item of {typeof(TTo).Name} is not {typeof(TItemTo).Name}");

			this.aSelectIdFrom = selectIdFrom;
			this.aSelectIdTo = selectIdTo;

			return this;
		}

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingDefinitionInformation mappingDefinition)
		{
			switch (this.aMappingType)
			{
				case CollectionMappingType.List:
					return new EnumerableToList<TFrom, TTo>(mappingDefinition);
				case CollectionMappingType.Array when this.aSelectIdFrom != null && this.aSelectIdTo != null:
					return new EnumerableToArrayWithSync<TFrom, TTo>(mappingDefinition, this.aSelectIdFrom, this.aSelectIdTo);
				case CollectionMappingType.Array:
					return new EnumerableToArrayWithMap<TFrom, TTo>(mappingDefinition);
				default:
					return new EnumerableToEnumerable<TFrom, TTo>(mappingDefinition);
			}
		}

		#endregion
	}

	public class CollectionMappingRules : IMappingRules
	{
		private Delegate aSelectIdFrom;
		private Delegate aSelectIdTo;
		private CollectionMappingType aMappingType;

		public CollectionMappingRules()
		{
			this.aMappingType = CollectionMappingType.Enumerable;
			this.aSelectIdFrom = null;
			this.aSelectIdTo = null;
		}

		public CollectionMappingRules MapAs(CollectionMappingType type)
		{
			this.aMappingType = type;

			return this;
		}

		public CollectionMappingRules EntityId<TItemFrom, TItemTo, TItemId>(Func<TItemFrom, TItemId> selectIdFrom, Func<TItemTo, TItemId> selectIdTo)
		{
			this.aSelectIdFrom = selectIdFrom;
			this.aSelectIdTo = selectIdTo;

			return this;
		}

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingDefinitionInformation mappingDefinition)
		{
			if (this.aSelectIdFrom != null && this.aSelectIdTo != null)
			{
				Type itemFrom = this.aSelectIdFrom.Method.GetParameters()[0].ParameterType;
				Type itemTo = this.aSelectIdTo.Method.GetParameters()[0].ParameterType;

				if (!typeof(IEnumerable<>).MakeGenericType(itemFrom).IsAssignableFrom(typeof(TFrom)))
					throw new InvalidMappingException($"Collection item of {typeof(TFrom).Name} is not {itemFrom.Name}");
				if (!typeof(IEnumerable<>).MakeGenericType(itemTo).IsAssignableFrom(typeof(TTo)))
					throw new InvalidMappingException($"Collection item of {typeof(TTo).Name} is not {itemTo.Name}");
			}

			switch (this.aMappingType)
			{
				case CollectionMappingType.List:
					return new EnumerableToList<TFrom, TTo>(mappingDefinition);
				case CollectionMappingType.Array when this.aSelectIdFrom != null && this.aSelectIdTo != null:
					return new EnumerableToArrayWithSync<TFrom, TTo>(mappingDefinition, this.aSelectIdFrom, this.aSelectIdTo);
				case CollectionMappingType.Array:
					return new EnumerableToArrayWithMap<TFrom, TTo>(mappingDefinition);
				default:
					return new EnumerableToEnumerable<TFrom, TTo>(mappingDefinition);
			}
		}

		#endregion
	}
}
