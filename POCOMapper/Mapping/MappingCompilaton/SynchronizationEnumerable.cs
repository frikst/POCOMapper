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
			Dictionary<TId, TItemTo> index = this.aTo.ToDictionary(this.aSelectIdTo);

			foreach (TItemFrom itemFrom in this.aFrom)
			{
				TItemTo itemTo;
				if (index.TryGetValue(this.aSelectIdFrom(itemFrom), out itemTo))
					itemTo = this.aMapping.Synchronize(itemFrom, itemTo);
				else
					itemTo = this.aMapping.Map(itemFrom);

				yield return itemTo;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
