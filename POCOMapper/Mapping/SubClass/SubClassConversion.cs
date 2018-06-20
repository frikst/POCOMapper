using System;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.SubClass
{
	internal class SubClassConversion : ISubClassConversionMapping
	{
		public SubClassConversion(Type from, Type to, IMapping mapping)
		{
			this.From = from;
			this.To = to;
			this.Mapping = mapping;
		}

		#region Implementation of ISubClassConversionMapping

		public Type From { get; }
		public Type To { get; }
		public IMapping Mapping { get; }

		#endregion
	}
}
