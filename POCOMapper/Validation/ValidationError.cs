using System;
using System.Reflection;

namespace KST.POCOMapper.Validation
{
	public abstract class ValidationError
	{
		private ValidationError(Type typeFrom, Type typeTo)
		{
			this.TypeFrom = typeFrom;
			this.TypeTo = typeTo;
		}

		public Type TypeFrom { get; }

		public Type TypeTo { get; }

		public class ExtraMappingFrom : ValidationError
		{
			public ExtraMappingFrom(Type typeFrom, MemberInfo memberFrom, Type typeTo, MemberInfo memberTo)
				: base(typeFrom, typeTo)
			{
				this.MemberFrom = memberFrom;
				this.MemberTo = memberTo;
			}

			public MemberInfo MemberFrom { get; }

			public MemberInfo MemberTo { get; }
		}

		public class ExtraMappingTo : ValidationError
		{
			public ExtraMappingTo(Type typeFrom, MemberInfo memberFrom, Type typeTo, MemberInfo memberTo)
				: base(typeFrom, typeTo)
			{
				this.MemberFrom = memberFrom;
				this.MemberTo = memberTo;
			}

			public MemberInfo MemberFrom { get; }

			public MemberInfo MemberTo { get; }
		}

		public class MissingMappingFrom : ValidationError
		{
			public MissingMappingFrom(Type typeFrom, MemberInfo memberFrom, Type typeTo)
				: base(typeFrom, typeTo)
			{
				this.MemberFrom = memberFrom;
			}

			public MemberInfo MemberFrom { get; }
		}

		public class MissingMappingTo : ValidationError
		{
			public MissingMappingTo(Type typeFrom, Type typeTo, MemberInfo memberTo)
				: base(typeFrom, typeTo)
			{
				this.MemberTo = memberTo;
			}

			public MemberInfo MemberTo { get; }
		}
	}
}
