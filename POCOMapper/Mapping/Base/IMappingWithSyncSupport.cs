namespace KST.POCOMapper.Mapping.Base
{
	public interface IMappingWithSyncSupport : IMapping
	{
		bool SynchronizeCanChangeObject { get; }
	}

	public interface IMappingWithSyncSupport<TFrom, TTo> : IMapping<TFrom, TTo>, IMappingWithSyncSupport
	{
		TTo Synchronize(TFrom from, TTo to);
	}
}
