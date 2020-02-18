using System;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Collection;
using KST.POCOMapper.Mapping.Decorators;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Mapping.SubClass;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Validation
{
	public class MappingValidationVisitor : IMappingVisitor
	{
		private readonly MappingErrorsLoggingVisitor aInnerVisitor;

		public MappingValidationVisitor()
		{
			this.aInnerVisitor = new MappingErrorsLoggingVisitor(Throw);
		}

		private void Throw(ValidationError validationError)
		{
			switch (validationError)
			{
				case ValidationError.ExtraMappingFrom error:
					throw new MappingValidationException($"{error.TypeFrom.Name}.{error.MemberFrom.Name} should not be mapped (mapping to {error.TypeTo.Name}.{error.MemberTo.Name} found)");
				case ValidationError.ExtraMappingTo error:
					throw new MappingValidationException($"{error.TypeTo.Name}.{error.MemberTo.Name} should not be mapped (mapping from {error.TypeFrom.Name}.{error.MemberFrom.Name} found)");
				case ValidationError.MissingMappingFrom error:
					throw new MappingValidationException($"{error.TypeFrom.Name}.{error.MemberFrom} should be mapped, but no mapping found (mapping to {error.TypeTo.Name})");
				case ValidationError.MissingMappingTo error:
					throw new MappingValidationException($"{error.TypeTo.Name}.{error.MemberTo} should be mapped, but no mapping found (mapping from {error.TypeFrom.Name})");
				default:
					throw new InvalidOperationException();
			}
		}

		#region Implementation of IMappingVisitor

		public void Begin()
		{
			this.aInnerVisitor.Begin();
		}

		public void Next()
		{
			this.aInnerVisitor.Next();
		}

		public void End()
		{
			this.aInnerVisitor.End();
		}

		public void Visit(ICollectionMapping mapping)
		{
			this.aInnerVisitor.Visit(mapping);
		}

		public void Visit(IObjectMapping mapping)
		{
			this.aInnerVisitor.Visit(mapping);
		}

		public void Visit(ISubClassMapping mapping)
		{
			this.aInnerVisitor.Visit(mapping);
		}

		public void Visit(IDecoratorMapping mapping)
		{
			this.aInnerVisitor.Visit(mapping);
		}

		public void Visit(IMapping mapping)
		{
			this.aInnerVisitor.Visit(mapping);
		}

		#endregion
	}
}
