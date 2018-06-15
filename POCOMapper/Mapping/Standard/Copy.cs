using System;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Standard
{
	public class Copy<TFromTo> : IMapping<TFromTo, TFromTo>
	{
		public Copy(MappingImplementation mappings)
		{
		}

		#region Implementation of IMapping

		public void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public bool CanSynchronize
			=> false;

		public bool CanMap
			=> true;

		public bool IsDirect
			=> true;

		public bool SynchronizeCanChangeObject
			=> false;

		public string MappingSource
			=> null;

		public string SynchronizationSource
			=> null;

		public Type From
			=> typeof(TFromTo);

		public Type To
			=> typeof(TFromTo);

		#endregion

		#region Implementation of IMapping<TFrom,TTo>

		public TFromTo Map(TFromTo from)
		{
			return from;
		}

		public TFromTo Synchronize(TFromTo from, TFromTo to)
			=> throw new NotImplementedException();

		#endregion
	}
}
