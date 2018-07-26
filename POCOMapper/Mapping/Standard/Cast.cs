using System;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Standard
{
	public class Cast<TFrom, TTo> : IMapping<TFrom, TTo>
	{
		private readonly CastMappingCompiler<TFrom, TTo> aMappingExpression;

		public Cast()
		{
			if (!BasicNetTypes.IsCastable<TFrom, TTo>())
				throw new InvalidMappingException($"You can use CastMapping only on (implicitly or explicitly) castable types, not {typeof(TFrom).Name} and {typeof(TTo).Name}");

			this.aMappingExpression = new CastMappingCompiler<TFrom, TTo>();
		}

		public void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public Type From
			=> typeof(TFrom);

		public Type To
			=> typeof(TTo);

		public TTo Map(TFrom from)
		{
			return this.aMappingExpression.Map(from);
		}
	}
}
