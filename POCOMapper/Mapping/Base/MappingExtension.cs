namespace KST.POCOMapper.Mapping.Base
{
	public static class MappingExtension
	{
		#region Support classes

		private class UnresolvedMapping : IUnresolvedMapping
		{
			public UnresolvedMapping(IMapping resolvedMapping)
			{
				this.ResolvedMapping = resolvedMapping;
			}

			public IMapping ResolvedMapping { get; }
		}

		private class UnresolvedMapping<TFrom, TTo> : IUnresolvedMapping<TFrom, TTo>
		{
			public UnresolvedMapping(IMapping<TFrom, TTo> resolvedMapping)
			{
				this.ResolvedMapping = resolvedMapping;
			}

			public IMapping<TFrom, TTo> ResolvedMapping { get; }

			IMapping IUnresolvedMapping.ResolvedMapping
				=> this.ResolvedMapping;
		}

		#endregion

		public static IUnresolvedMapping AsUnresolved(this IMapping mapping)
			=> new UnresolvedMapping(mapping);

		public static IUnresolvedMapping<TFrom, TTo> AsUnresolved<TFrom, TTo>(this IMapping<TFrom, TTo> mapping)
			=> new UnresolvedMapping<TFrom, TTo>(mapping);
	}
}
