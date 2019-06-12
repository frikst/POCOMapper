namespace KST.POCOMapper.Mapping.Base
{
    public interface IMappingWithSpecialComparision : IMapping
    {

    }

    public interface IMappingWithSpecialComparision<TFrom, TTo> : IMapping<TFrom, TTo>, IMappingWithSpecialComparision
    {
        bool MapEqual(TFrom from, TTo to);
    }
}
