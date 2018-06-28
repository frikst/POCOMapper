using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Conventions.SymbolConventions;
using KST.POCOMapper.Members;
using KST.POCOMapper.TypePatterns.Group;

namespace KST.POCOMapper.Conventions
{
	public class ConditionalConventions : NamingConventions
	{
		private readonly PatternGroup aTypePatterns;

		internal ConditionalConventions(Direction conventionDirection, ISymbolConvention fields, ISymbolConvention methods, ISymbolConvention properties, IEnumerable<MemberType> memberScanningPrecedence, PatternGroup typePatterns)
			: base(conventionDirection, fields, methods, properties, memberScanningPrecedence)
		{
			this.aTypePatterns = typePatterns;
		}

		#region Overrides of Conventions

		public override IEnumerable<NamingConventions> GetChildConventions()
			=> Enumerable.Empty<NamingConventions>();

		public override bool CanPair(IMember first, IMember second)
		{
			if (this.ConventionDirection == Direction.From)
				return this.aTypePatterns.Matches(first.Type, second.Type);
			else
				return this.aTypePatterns.Matches(second.Type, first.Type);
		}

		#endregion
	}
}
