using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Decorators
{
    public interface IDecoratorMapping : IMapping
    {
		IMapping DecoratedMapping { get; }
    }
}
