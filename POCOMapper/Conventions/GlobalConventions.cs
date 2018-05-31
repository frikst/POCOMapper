using System;
using System.Collections.Generic;
using KST.POCOMapper.conventions.members;
using KST.POCOMapper.typePatterns;

namespace KST.POCOMapper.conventions
{
	public class GlobalConventions : Conventions
	{
		private readonly List<Conventions> aConditionalConventionList;

		internal GlobalConventions(Conventions.Direction direction)
			: base(direction)
		{
			this.aConditionalConventionList = new List<Conventions>();
		}

		public GlobalConventions ConditionalConventions<TMemberFrom, TMemberTo>(Action<Conventions> conventions)
		{
			Conventions conv = new ConditionalConventions(new Pattern<TMemberFrom>(), new Pattern<TMemberTo>(), this.ConventionDirection);
			conventions(conv);

			this.aConditionalConventionList.Add(conv);

			return this;
		}

		public GlobalConventions ConditionalConventions(IPattern from, IPattern to, Action<Conventions> conventions)
		{
			Conventions conv = new ConditionalConventions(from, to, this.ConventionDirection);
			conventions(conv);

			this.aConditionalConventionList.Add(conv);

			return this;
		}

		#region Overrides of Conventions

		public override IEnumerable<Conventions> GetChildConventions()
		{
			foreach (Conventions convention in this.aConditionalConventionList)
				yield return convention;
		}

		public override bool CanPair(IMember first, IMember second)
		{
			return true;
		}

		#endregion
	}
}
