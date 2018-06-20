namespace KST.POCOMapper.Mapping.Base
{
	public interface IUnresolvedMapping
	{
		IMapping ResolvedMapping { get; }
	}

	public interface IUnresolvedMapping<TFrom, TTo> : IUnresolvedMapping
	{
		new IMapping<TFrom, TTo> ResolvedMapping { get; }
	}
}
