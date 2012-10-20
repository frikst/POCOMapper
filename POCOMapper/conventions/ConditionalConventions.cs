using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMapper.conventions.members;

namespace POCOMapper.conventions
{
	public class ConditionalConventions : Conventions
	{
		private readonly Type aFrom;
		private readonly Type aTo;

		internal ConditionalConventions(Type from, Type to, Conventions.Direction direction)
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
				return this.aFrom.IsAssignableFrom(first.Type) && this.aTo.IsAssignableFrom(second.Type);
			else
				return this.aFrom.IsAssignableFrom(second.Type) && this.aTo.IsAssignableFrom(first.Type);
		}

		#endregion
	}
}
