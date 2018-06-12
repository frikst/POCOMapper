using System;
using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Conventions.Members;
using KST.POCOMapper.Conventions.Symbols;
using KST.POCOMapper.Exceptions;

namespace KST.POCOMapper.Conventions
{
	public abstract class NamingConventions
	{
		public enum Direction
		{
			From,
			To
		}

		private Func<Type, NamingConventions, IMember, IEnumerable<IMember>> aMemberIterator;
		private MemberType[] aMemberScanningPrecedence;

		protected NamingConventions(Direction direction)
		{
			this.ConventionDirection = direction;
			this.Fields = new BigCammelCase();
			this.Methods = new BigCammelCase();
			this.Properties = new BigCammelCase();

			this.aMemberIterator = (type, conventions, parent) => new MemberIterator(type, conventions, parent);

			this.aMemberScanningPrecedence = new MemberType[] { MemberType.Property, MemberType.Method, MemberType.Field };
		}

		public ISymbolParser Fields { get; private set; }
		public ISymbolParser Methods { get; private set; }
		public ISymbolParser Properties { get; private set; }

		public IEnumerable<IMember> GetAllMembers(Type type, IMember parent = null)
			=> this.aMemberIterator(type, this, parent);

		public NamingConventions SetFieldConvention(ISymbolParser parser)
		{
			this.Fields = parser;
			return this;
		}

		public NamingConventions SetMethodConvention(ISymbolParser parser)
		{
			this.Methods = parser;
			return this;
		}

		public NamingConventions SetPropertyConvention(ISymbolParser parser)
		{
			this.Properties = parser;
			return this;
		}

		public NamingConventions SetMemberIterator(Func<Type, NamingConventions, IMember, IEnumerable<IMember>> memberIterator)
		{
			this.aMemberIterator = memberIterator;
			return this;
		}

		public NamingConventions SetMemberScanningPrecedence(params MemberType[] precedence)
		{
			if (precedence.GroupBy(x => x).Any(x => x.Count() > 1))
				throw new InvalidConvention("Parameters of the SetMemberScanningPrecedence method must be uniqe");

			if (precedence.Any(x => x == MemberType.Property) && precedence.Any(x => x == MemberType.AutoProperty || x == MemberType.CodeProperty))
				throw new InvalidConvention("Parameters of the SetMemberScanningPrecedence method must be uniqe");

			this.aMemberScanningPrecedence = precedence;
			return this;
		}

		public IEnumerable<MemberType> GetMemberScanningPrecedence()
		{
			foreach (MemberType memberType in this.aMemberScanningPrecedence)
				yield return memberType;
		}

		public Direction ConventionDirection { get; }

		public abstract IEnumerable<NamingConventions> GetChildConventions();
		public abstract bool CanPair(IMember first, IMember second);
	}
}
