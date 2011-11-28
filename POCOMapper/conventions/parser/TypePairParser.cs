using System.Collections;
using System.Collections.Generic;
using System.Linq;
using POCOMapper.definition;
using POCOMapper.mapping.@base;

namespace POCOMapper.conventions.parser
{
	public class TypePairParser : IEnumerable<PairedMembers>
	{
		private readonly MappingImplementation aMapping;
		private readonly IEnumerable<IMember> aFrom;
		private readonly IEnumerable<IMember> aTo;

		public TypePairParser(MappingImplementation mapping, IEnumerable<IMember> from, IEnumerable<IMember> to)
		{
			this.aMapping = mapping;
			this.aFrom = from;
			this.aTo = to;
		}

		private PairedMembers DetectPair(IMember from, IMember to)
		{
			if (from.Symbol != to.Symbol)
				return null;

			IMapping mapping = this.aMapping.GetMapping(from.Type, to.Type);

			if (mapping != null)
				return new PairedMembers(from, to, mapping);
			else if (from.Type == to.Type)
				return new PairedMembers(from, to, null);
			else
				return null;
		}

		#region Implementation of IEnumerable

		public IEnumerator<PairedMembers> GetEnumerator()
		{
			List<IMember> fromAll = this.aFrom.Where(x => x.Getter != null).ToList();
			List<IMember> toAll = this.aTo.Where(x => x.Setter != null).ToList();

			foreach (IMember fromOne in fromAll)
			{
				foreach (IMember toOne in toAll)
				{
					PairedMembers pair = this.DetectPair(fromOne, toOne);

					if (pair != null)
					{
						yield return pair;
						break;
					}
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
