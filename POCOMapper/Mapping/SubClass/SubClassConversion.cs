using System;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.SubClass
{
	internal class SubClassConversion : ISubClassConversionMapping
	{
		public SubClassConversion(Type from, Type to, IUnresolvedMapping mapping)
		{
			this.From = from;
			this.To = to;
			this.Mapping = mapping;
		}

		public IUnresolvedMapping Mapping { get; }

		#region Implementation of ISubClassConversionMapping

		public Type From { get; }
		public Type To { get; }
		IMapping ISubClassConversionMapping.Mapping
			=> this.Mapping.ResolvedMapping;

		#endregion
	}
}
