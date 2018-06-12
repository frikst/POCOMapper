using System;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Standard
{
	public class ToString<TFrom> : IMapping<TFrom, string>
	{
		public ToString(MappingImplementation mappings)
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
			=> false;

		public bool SynchronizeCanChangeObject
			=> false;

		public string MappingSource
			=> null;

		public string SynchronizationSource
			=> null;

		public Type From
			=> typeof(TFrom);

		public Type To
			=> typeof(string);

		#endregion

		#region Implementation of IMapping<TFrom,string>

		public string Map(TFrom from)
		{
			return from.ToString();
		}

		public string Synchronize(TFrom from, string to)
			=> throw new NotImplementedException();

		#endregion
	}
}
