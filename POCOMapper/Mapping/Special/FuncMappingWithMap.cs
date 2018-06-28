using System;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Special
{
	public class FuncMappingWithMap<TFrom, TTo> : IMapping<TFrom, TTo>
	{
		private readonly Func<TFrom, TTo> aMappingFunc;

		public FuncMappingWithMap(Func<TFrom, TTo> mappingFunc)
		{
			this.aMappingFunc = mappingFunc;
		}

		public void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public bool CanMap
			=> true;

		public bool IsDirect
			=> false;

		public Type From
			=> typeof(TFrom);

		public Type To
			=> typeof(TTo);

		public TTo Map(TFrom from)
		{
			return this.aMappingFunc(from);
		}
	}
}
