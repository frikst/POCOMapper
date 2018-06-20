using System;
using System.Linq.Expressions;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Standard
{
	public class Parse<TTo> : IMapping<string, TTo>
	{
		private readonly ParseMappingCompiler<TTo> aMappingExpression;

		public Parse(MappingImplementation mapping)
		{
			if (!typeof(TTo).IsEnum && !typeof(TTo).IsPrimitive)
				throw new InvalidMappingException($"You can use Parse only on primitive or enum types, not {typeof(TTo).Name}");

			this.aMappingExpression = new ParseMappingCompiler<TTo>();
		}

		public void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public bool CanSynchronize
			=> false;

		public bool CanMap
			=> true;

		public bool IsDirect
			=> false;

		public bool SynchronizeCanChangeObject
			=> false;

		public string MappingSource
			=> this.aMappingExpression.Source;

		public string SynchronizationSource
			=> null;

		public Type From
			=> typeof(string);

		public Type To
			=> typeof(TTo);

		public TTo Map(string from)
		{
			return this.aMappingExpression.Map(from);
		}

		public TTo Synchronize(string from, TTo to)
		{
			throw new NotImplementedException();
		}
	}
}
