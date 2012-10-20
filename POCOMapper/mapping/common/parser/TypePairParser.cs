using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using POCOMapper.conventions.members;
using POCOMapper.conventions.symbol;
using POCOMapper.definition;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.common.parser
{
	public class TypePairParser : IEnumerable<PairedMembers>
	{
		private enum Found
		{
			None,
			Left,
			Right,
			Full
		}

		private readonly MappingImplementation aMapping;
		private readonly Type aFrom;
		private readonly Type aTo;
		private readonly Dictionary<Type, List<IMember>> aFromMembers;
		private readonly Dictionary<Type, List<IMember>> aToMembers;

		public TypePairParser(MappingImplementation mapping, Type from, Type to)
		{
			this.aMapping = mapping;
			this.aFrom = from;
			this.aTo = to;

			this.aFromMembers = new Dictionary<Type, List<IMember>>();
			this.aToMembers = new Dictionary<Type, List<IMember>>();
		}

		private PairedMembers CreateMemberPair(IMember from, IMember to)
		{
			IMapping mapping = this.aMapping.GetMapping(from.Type, to.Type);

			if (mapping != null)
				return new PairedMembers(from, to, mapping);
			else
				return null;
		}

		private PairedMembers DetectPair(IMember from, IMember to)
		{
			if (from.Symbol != to.Symbol || !from.CanPairWith(to))
				return null;

			return CreateMemberPair(from, to);
		}

		private PairedMembers DetectPairLeft(IMember from, IMember to, Symbol foundPart)
		{
			IMember foundMember = null;

			foreach (IMember toOne in this.GetToMembers(to.Type, to))
			{
				if (from.Symbol == foundPart + toOne.Symbol && from.CanPairWith(toOne))
					return this.CreateMemberPair(from, toOne);
				else if (from.Symbol.HasPrefix(toOne.Symbol))
					foundMember = toOne;
			}

			if (foundMember != null)
				return this.DetectPairLeft(from, foundMember, foundPart + foundMember.Symbol);

			return null;
		}

		private PairedMembers DetectPairRight(IMember from, IMember to, Symbol foundPart)
		{
			IMember foundMember = null;

			foreach (IMember fromOne in this.GetToMembers(from.Type, from))
			{
				if (to.Symbol == foundPart + fromOne.Symbol && fromOne.CanPairWith(to))
					return this.CreateMemberPair(fromOne, to);
				else if (to.Symbol.HasPrefix(fromOne.Symbol))
					foundMember = fromOne;
			}

			if (foundMember != null)
				return this.DetectPairLeft(foundMember, to, foundPart + foundMember.Symbol);

			return null;
		}

		private IEnumerable<IMember> GetFromMembers(Type type)
		{
			List<IMember> value;
			if (this.aFromMembers.TryGetValue(type, out value))
				return value;

			value = this.aMapping.FromConventions.GetAllMembers(type).Where(x => x.Getter != null).ToList();
			this.aFromMembers[type] = value;
			return value;
		}

		private IEnumerable<IMember> GetToMembers(Type type, IMember parent = null)
		{
			List<IMember> value;
			if (this.aToMembers.TryGetValue(type, out value))
				return value;

			value = this.aMapping.ToConventions.GetAllMembers(type, parent).Where(x => x.Setter != null).ToList();
			this.aToMembers[type] = value;
			return value;
		}

		#region Implementation of IEnumerable

		public IEnumerator<PairedMembers> GetEnumerator()
		{
			IEnumerable<IMember> fromAll = this.GetFromMembers(this.aFrom);
			IEnumerable<IMember> toAll = this.GetToMembers(this.aTo);

			foreach (IMember fromOne in fromAll)
			{
				Found found = Found.None;
				IMember foundMember = null;

				foreach (IMember toOne in toAll)
				{
					PairedMembers pair = this.DetectPair(fromOne, toOne);

					if (pair != null)
					{
						found = Found.Full;

						yield return pair;
						break;
					}
					else if (fromOne.Symbol.HasPrefix(toOne.Symbol))
					{
						found = Found.Left;
						foundMember = toOne;
					}
					else if (toOne.Symbol.HasPrefix(fromOne.Symbol))
					{
						found = Found.Right;
						foundMember = toOne;
					}
				}

				if (foundMember != null)
				{
					PairedMembers pair;

					switch (found)
					{
						case Found.Left:
							pair = this.DetectPairLeft(fromOne, foundMember, foundMember.Symbol);
							if (pair != null)
								yield return pair;

							break;
						case Found.Right:
							pair = this.DetectPairRight(fromOne, foundMember, fromOne.Symbol);
							if (pair != null)
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
