using System;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Executor
{
	internal class UnresolvedMapping<TFrom, TTo> : IUnresolvedMapping<TFrom, TTo>
	{
		private IMapping<TFrom, TTo> aResolvedMapping;
		private readonly MappingContainer aMappingContainer;

		public UnresolvedMapping(MappingContainer mappingContainer)
		{
			this.aMappingContainer = mappingContainer;
			this.aResolvedMapping = null;
		}

		IMapping IUnresolvedMapping.ResolvedMapping
			=> this.ResolvedMapping;

		public IMapping<TFrom, TTo> ResolvedMapping
		{
			get
			{
				if (this.aResolvedMapping == null)
					this.aResolvedMapping = this.aMappingContainer.GetMapping<TFrom, TTo>();

				return this.aResolvedMapping;
			}
		}
	}

	internal class UnresolvedMapping : IUnresolvedMapping
	{
		private IMapping aResolvedMapping;
		private readonly MappingContainer aMappingContainer;
		private readonly Type aFrom;
		private readonly Type aTo;

		public UnresolvedMapping(MappingContainer mappingContainer, Type from, Type to)
		{
			this.aMappingContainer = mappingContainer;
			this.aFrom = from;
			this.aTo = to;
			this.aResolvedMapping = null;
		}

		public IMapping ResolvedMapping
		{
			get
			{
				if (this.aResolvedMapping == null)
					this.aResolvedMapping = this.aMappingContainer.GetMapping(this.aFrom, this.aTo);

				return this.aResolvedMapping;
			}
		}
	}
}
