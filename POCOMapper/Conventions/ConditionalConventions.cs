using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMapper.conventions.members;
using POCOMapper.typePatterns;

namespace POCOMapper.conventions
{
	public class ConditionalConventions : Conventions
	{
		private readonly IPattern aFrom;
		private readonly IPattern aTo;

		internal ConditionalConventions(IPattern from, IPattern to, Conventions.Direction direction)
			: base(direction)
		{
			this.aFrom = from;
			this.aTo = to;
		}

		#region Overrides of Conventions

		public override IEnumerable<Conventions> GetChildConventions()
		{
			return Enumerable.Empty<Conventions>();
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
