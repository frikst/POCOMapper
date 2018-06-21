namespace KST.POCOMapper.Mapping.Base
{
	public static class MappingExtension
	{
		private class ReallyResolvedMapping : IUnresolvedMapping
		{
			public ReallyResolvedMapping(IMapping resolvedMapping)
			{
				this.ResolvedMapping = resolvedMapping;
			}

			public IMapping ResolvedMapping { get; }
		}

		private class ReallyResolvedMapping<TFrom, TTo> : IUnresolvedMapping<TFrom, TTo>
		{
			public ReallyResolvedMapping(IMapping<TFrom, TTo> resolvedMapping)
			{
				this.ResolvedMapping = resolvedMapping;
			}

			public IMapping<TFrom, TTo> ResolvedMapping { get; }

			IMapping IUnresolvedMapping.ResolvedMapping
				=> this.ResolvedMapping;
		}

		public static IUnresolvedMapping AsUnresolved(this IMapping mapping)
			=> new ReallyResolvedMapping(mapping);

		public static IUnresolvedMapping<TFrom, TTo> AsUnresolved<TFrom, TTo>(this IMapping<TFrom, TTo> mapping)
			=> new ReallyResolvedMapping<TFrom, TTo>(mapping);
	}
}
