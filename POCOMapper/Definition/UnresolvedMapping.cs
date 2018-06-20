using System;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Definition
{
	internal class UnresolvedMapping<TFrom, TTo> : IUnresolvedMapping<TFrom, TTo>
	{
		private IMapping<TFrom, TTo> aResolvedMapping;
		private readonly MappingImplementation aMappingImplementation;

		public UnresolvedMapping(MappingImplementation mappingImplementation)
		{
			this.aMappingImplementation = mappingImplementation;
			this.aResolvedMapping = null;
		}

		IMapping IUnresolvedMapping.ResolvedMapping
			=> this.ResolvedMapping;

		public IMapping<TFrom, TTo> ResolvedMapping
		{
			get
			{
				if (this.aResolvedMapping == null)
					this.aResolvedMapping = this.aMappingImplementation.GetMapping<TFrom, TTo>();

				return this.aResolvedMapping;
			}
		}
	}

	internal class UnresolvedMapping : IUnresolvedMapping
	{
		private IMapping aResolvedMapping;
		private readonly MappingImplementation aMappingImplementation;
		private readonly Type aFrom;
		private readonly Type aTo;

		public UnresolvedMapping(MappingImplementation mappingImplementation, Type from, Type to)
		{
			this.aMappingImplementation = mappingImplementation;
			this.aFrom = from;
			this.aTo = to;
			this.aResolvedMapping = null;
		}

		public IMapping ResolvedMapping
		{
			get
			{
				if (this.aResolvedMapping == null)
					this.aResolvedMapping = this.aMappingImplementation.GetMapping(this.aFrom, this.aTo);

				return this.aResolvedMapping;
			}
		}
	}
}
