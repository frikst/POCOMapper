using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMapper.definition;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.standard
{
	public class Copy<TFromTo> : IMapping<TFromTo, TFromTo>
	{
		public Copy(MappingImplementation mappings)
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

		#endregion

		#region Implementation of IMapping<TFrom,TTo>

		public TFromTo Map(TFromTo from)
		{
			return from;
		}

		public void Synchronize(TFromTo from, TFromTo to)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
