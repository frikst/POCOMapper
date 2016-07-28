using POCOMapper.mapping.@base;
using POCOMapper.mapping.collection;
using POCOMapper.mapping.common;

namespace POCOMapper.visitor
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
