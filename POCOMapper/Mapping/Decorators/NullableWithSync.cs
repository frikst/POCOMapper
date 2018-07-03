using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Decorators
{
	public class NullableWithSync<TFrom, TTo> : NullableWithMap<TFrom, TTo>, IMappingWithSyncSupport<TFrom, TTo>
		where TFrom : class
		where TTo : class
	{
		private readonly IMappingWithSyncSupport<TFrom, TTo> aInnerMapping;

		public NullableWithSync(IMappingWithSyncSupport<TFrom, TTo> innerMapping)
			: base(innerMapping)
		{
			this.aInnerMapping = innerMapping;
		}

		public bool SynchronizeCanChangeObject
			=> true;

		public TTo Synchronize(TFrom from, TTo to)
		{
			if (from == null)
				return null;

			if (to == null)
				return this.Map(from);

			return this.aInnerMapping.Synchronize(from, to);
		}
	}
}