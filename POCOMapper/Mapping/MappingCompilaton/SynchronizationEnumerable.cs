using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.MappingCompilaton
{
	internal class SynchronizationEnumerable<TId, TItemFrom, TItemTo> : IEnumerable<TItemTo>
	{
		private readonly Func<TItemTo, TId> aSelectIdTo;
		private readonly Func<TItemFrom, TId> aSelectIdFrom;

		private readonly IEnumerable<TItemFrom> aFrom;
		private readonly IEnumerable<TItemTo> aTo;

		private readonly IMapping<TItemFrom, TItemTo> aMapping;

		public SynchronizationEnumerable(IEnumerable<TItemFrom> from, IEnumerable<TItemTo> to, Func<TItemTo, TId> selectIdTo, Func<TItemFrom, TId> selectIdFrom, IMapping<TItemFrom, TItemTo> mapping)
		{
			this.aFrom = from;
			this.aTo = to;

			this.aSelectIdTo = selectIdTo;
			this.aSelectIdFrom = selectIdFrom;

			this.aMapping = mapping;
		}

		#region Implementation of IEnumerable

		public IEnumerator<TItemTo> GetEnumerator()
		{
			if (this.aMapping is IMappingWithSyncSupport<TItemFrom, TItemTo> mappingWithSync)
				return this.EnumerateWithSync(mappingWithSync);
			else
				return this.EnumerateWithMap(this.aMapping);
		}

		private IEnumerator<TItemTo> EnumerateWithSync(IMappingWithSyncSupport<TItemFrom, TItemTo> mappingWithSync)
		{
			var index = this.aTo.ToDictionary(this.aSelectIdTo);

			foreach (TItemFrom itemFrom in this.aFrom)
			{
				TItemTo itemTo;
				if (index.TryGetValue(this.aSelectIdFrom(itemFrom), out itemTo))
					itemTo = mappingWithSync.Synchronize(itemFrom, itemTo);
				else
					itemTo = mappingWithSync.Map(itemFrom);

				yield return itemTo;
			}
		}

		private IEnumerator<TItemTo> EnumerateWithMap(IMapping<TItemFrom, TItemTo> mappingWithMap)
		{
			return this.aFrom.Select(mappingWithMap.Map).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
