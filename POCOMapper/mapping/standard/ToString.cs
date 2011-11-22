using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
