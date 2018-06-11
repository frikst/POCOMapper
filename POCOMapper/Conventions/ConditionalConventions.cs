using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Conventions.Members;
using KST.POCOMapper.TypePatterns;

namespace KST.POCOMapper.Conventions
{
	public class ConditionalConventions : NamingConventions
	{
		private readonly IPattern aFrom;
		private readonly IPattern aTo;

		internal ConditionalConventions(IPattern from, IPattern to, NamingConventions.Direction direction)
			: base(direction)
		{
			this.aFrom = from;
			this.aTo = to;
		}

		#region Overrides of Conventions

		public override IEnumerable<NamingConventions> GetChildConventions()
		{
			return Enumerable.Empty<NamingConventions>();
		}

		public override bool CanPair(IMember first, IMember second)
		{
			if (this.ConventionDirection == Direction.From)
				return this.aFrom.Matches(first.Type) && this.aTo.Matches(second.Type);
			else
				return this.aFrom.Matches(second.Type) && this.aTo.Matches(first.Type);
		}

		#endregion
	}
}
