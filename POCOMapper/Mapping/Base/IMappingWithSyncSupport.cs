namespace KST.POCOMapper.Mapping.Base
{
	public interface IMappingWithSyncSupport : IMapping
	{
		bool SynchronizeCanChangeObject { get; }

		string SynchronizationSource { get; }
	}

	public interface IMappingWithSyncSupport<TFrom, TTo> : IMapping<TFrom, TTo>, IMappingWithSyncSupport
	{
		TTo Synchronize(TFrom from, TTo to);
	}
}
