using System;
using System.Collections.Generic;
using System.Linq;
using POCOMapper.definition;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.standard
{
	public class ToString<TFrom> : IMapping<TFrom, string>
	{
		public ToString(MappingImplementation mappings)
		{
		}

		#region Implementation of IMapping

		public IEnumerable<Tuple<string, IMapping>> Children
		{
			get { return Enumerable.Empty<Tuple<string, IMapping>>(); }
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

		public string MappingSource
		{
			get { return null; }
		}

		public string SynchronizationSource
		{
			get { return null; }
		}

		#endregion

		#region Implementation of IMapping<TFrom,string>

		public string Map(TFrom from)
		{
			return from.ToString();
		}

		public void Synchronize(TFrom from, string to)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
