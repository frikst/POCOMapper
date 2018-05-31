using KST.POCOMapper.mapping.@base;
using KST.POCOMapper.mapping.collection;
using KST.POCOMapper.mapping.common;

namespace KST.POCOMapper.visitor
{
	public interface IMappingVisitor
	{
		void Begin();
		void Next();
		void End();

		void Visit(ICollectionMapping mapping);
		void Visit(IObjectMapping mapping);
		void Visit(ISubClassMapping mapping);

		void Visit(IMapping mapping);
	}
}
