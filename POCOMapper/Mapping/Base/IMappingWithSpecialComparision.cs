namespace KST.POCOMapper.Mapping.Base
{
    public interface IMappingWithSpecialComparision<TFrom, TTo> : IMapping<TFrom, TTo>
    {
        bool MapEqual(TFrom from, TTo to);
    }
}
