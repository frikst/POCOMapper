namespace KST.POCOMapper.Mapping.Base
{
    public interface IDirectMapping : IMapping
    {
    }

	public interface IDirectMapping<TFromTo> : IMapping<TFromTo, TFromTo>, IDirectMapping
	{

	}
}
