using System;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Standard
{
	public class Parse<TTo> : IMapping<string, TTo>
	{
		private readonly ParseMappingCompiler<TTo> aMappingExpression;

		public Parse()
		{
			if (!typeof(TTo).IsEnum && !typeof(TTo).IsPrimitive)
				throw new InvalidMappingException($"You can use Parse only on primitive or enum types, not {typeof(TTo).Name}");

			this.aMappingExpression = new ParseMappingCompiler<TTo>();
		}

		public void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public bool CanMap
			=> true;

		public bool IsDirect
			=> false;

		public string MappingSource
			=> this.aMappingExpression.Source;

		public Type From
			=> typeof(string);

		public Type To
			=> typeof(TTo);

		public TTo Map(string from)
		{
			return this.aMappingExpression.Map(from);
		}
	}
}
