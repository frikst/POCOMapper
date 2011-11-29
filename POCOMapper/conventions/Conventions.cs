using System;
using System.Collections.Generic;
using POCOMapper.conventions.members;
using POCOMapper.conventions.symbol;

namespace POCOMapper.conventions
{
	public class Conventions
	{
		private Func<Type, Conventions, IEnumerable<IMember>> aMemberIterator;

		public Conventions()
		{
			this.Attributes = new BigCammelCase();
			this.Methods = new BigCammelCase();
			this.Properties = new BigCammelCase();

			this.aMemberIterator = (type, conventions) => new MemberIterator(type, conventions);
		}

		public ISymbolParser Attributes { get; private set; }
		public ISymbolParser Methods { get; private set; }
		public ISymbolParser Properties { get; private set; }

		public IEnumerable<IMember> GetAllMembers(Type type)
		{
			return this.aMemberIterator(type, this);
		}

		public Conventions SetAttributeConvention(ISymbolParser parser)
		{
			this.Attributes = parser;
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

		public Conventions SetMemberIterator(Func<Type, Conventions, IEnumerable<IMember>> memberIterator)
		{
			this.aMemberIterator = memberIterator;
			return this;
		}
	}
}
