using System;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Standard
{
	public class Copy<TFromTo> : IDirectMapping<TFromTo>
	{
		public void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public Type From
			=> typeof(TFromTo);

		public Type To
			=> typeof(TFromTo);

		public TFromTo Map(TFromTo from)
		{
			return from;
		}
	}
}
