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
		{
			get { return false; }
		}

		public bool CanMap
		{
			get { return true; }
		}

		public bool IsDirect
		{
			get
			{
				if (typeof(TFromTo) == typeof(string))
					return true;
				else if (typeof(TFromTo).IsValueType)
					return true;
				else
					return false;
			}
		}

		public bool SynchronizeCanChangeObject
		{
			get { return false; }
		}

		public string MappingSource
		{
			get { return null; }
		}

		public string SynchronizationSource
		{
			get { return null; }
		}

		public Type From
		{
			get { return typeof(TFromTo); }
		}

		public Type To
		{
			get { return typeof(TFromTo); }
		}

		#endregion

		#region Implementation of IMapping<TFrom,TTo>

		public TFromTo Map(TFromTo from)
		{
			return from;
		}

		public TFromTo Synchronize(TFromTo from, TFromTo to)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
