namespace KST.POCOMapper.Mapping.Base
{
    public interface IMappingWithComparisionSupport<TFrom, TTo> : IMapping<TFrom, TTo>
    {
        bool MapEqual(TFrom from, TTo to);
    }
}
