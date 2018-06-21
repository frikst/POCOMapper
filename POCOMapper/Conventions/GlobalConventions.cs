using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Conventions.SymbolConventions;
using KST.POCOMapper.Members;

namespace KST.POCOMapper.Conventions
{
	public class GlobalConventions : NamingConventions
	{
		private readonly NamingConventions[] aConditionalConventionList;

		internal GlobalConventions(Direction conventionDirection, ISymbolConvention fields, ISymbolConvention methods, ISymbolConvention properties, IEnumerable<MemberType> memberScanningPrecedence, IEnumerable<NamingConventions> conditionalConventionList)
			: base(conventionDirection, fields, methods, properties, memberScanningPrecedence)
		{
			this.aConditionalConventionList = conditionalConventionList.ToArray();
		}

		#region Overrides of Conventions

		public override IEnumerable<NamingConventions> GetChildConventions()
			=> this.aConditionalConventionList;

		public override bool CanPair(IMember first, IMember second)
			=> true;

		#endregion
	}
}
