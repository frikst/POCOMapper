using System;
using System.Collections.Generic;
using System.Linq;
using POCOMapper.definition;
using POCOMapper.mapping.@base;
using POCOMapper.visitor;

namespace POCOMapper.mapping.standard
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
		{
			get { return false; }
		}

		public bool CanMap
		{
			get { return true; }
		}

		public bool IsDirect
		{
			get { return false; }
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
			get { return typeof(TFrom); }
		}

		public Type To
		{
			get { return typeof(string); }
		}

		#endregion

		#region Implementation of IMapping<TFrom,string>

		public string Map(TFrom from)
		{
			return from.ToString();
		}

		public string Synchronize(TFrom from, string to)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
