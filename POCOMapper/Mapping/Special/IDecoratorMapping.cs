using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Special
{
    public interface IDecoratorMapping : IMapping
    {
		IMapping DecoratedMapping { get; }
    }
}
