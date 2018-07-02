using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Collection;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Mapping.Special;
using KST.POCOMapper.Mapping.SubClass;

namespace KST.POCOMapper.Visitor
{
	public interface IMappingVisitor
	{
		void Begin();
		void Next();
		void End();

		void Visit(ICollectionMapping mapping);
		void Visit(IObjectMapping mapping);
		void Visit(ISubClassMapping mapping);
		void Visit(IDecoratorMapping mapping);

		void Visit(IMapping mapping);
	}
}
