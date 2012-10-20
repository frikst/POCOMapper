using System;
using System.Linq;
using System.Collections.Generic;
using POCOMapper.conventions.members;
using POCOMapper.conventions.symbol;
using POCOMapper.exceptions;

namespace POCOMapper.conventions
{
	public abstract class Conventions
	{
		public enum Direction
		{
			From,
			To
		}

		private Func<Type, Conventions, IMember, IEnumerable<IMember>> aMemberIterator;
		private MemberType[] aMemberScanningPrecedence;

		protected Conventions(Direction direction)
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
		{
			return this.aMemberIterator(type, this, parent);
		}

		public Conventions SetFieldConvention(ISymbolParser parser)
		{
			this.Fields = parser;
			return this;
		}

		public Conventions SetMethodConvention(ISymbolParser parser)
		{
			this.Methods = parser;
			return this;
		}

		public Conventions SetPropertyConvention(ISymbolParser parser)
		{
			this.Properties = parser;
			return this;
		}

		public Conventions SetMemberIterator(Func<Type, Conventions, IMember, IEnumerable<IMember>> memberIterator)
		{
			this.aMemberIterator = memberIterator;
			return this;
		}

		public Conventions SetMemberScanningPrecedence(params MemberType[] precedence)
		{
			if (precedence.GroupBy(x => x).Any(x => x.Count() > 1))
				throw new InvalidConvention("Parameters of the SetMemberScanningPrecedence method must be uniqe");

			this.aMemberScanningPrecedence = precedence;
			return this;
		}

		public IEnumerable<MemberType> GetMemberScanningPrecedence()
		{
			foreach (MemberType memberType in this.aMemberScanningPrecedence)
				yield return memberType;
		}

		public Direction ConventionDirection { get; private set; }

		public abstract IEnumerable<Conventions> GetChildConventions();
		public abstract bool CanPair(IMember first, IMember second);
	}
}
