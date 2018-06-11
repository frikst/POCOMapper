using System;
using System.Collections.Generic;
using KST.POCOMapper.Conventions.Members;
using KST.POCOMapper.TypePatterns;

namespace KST.POCOMapper.Conventions
{
	public class GlobalConventions : NamingConventions
	{
		private readonly List<NamingConventions> aConditionalConventionList;

		internal GlobalConventions(NamingConventions.Direction direction)
			: base(direction)
		{
			this.aConditionalConventionList = new List<NamingConventions>();
		}

		public GlobalConventions ConditionalConventions<TMemberFrom, TMemberTo>(Action<NamingConventions> conventions)
		{
			NamingConventions conv = new ConditionalConventions(new Pattern<TMemberFrom>(), new Pattern<TMemberTo>(), this.ConventionDirection);
			conventions(conv);

			this.aConditionalConventionList.Add(conv);

			return this;
		}

		public GlobalConventions ConditionalConventions(IPattern from, IPattern to, Action<NamingConventions> conventions)
		{
			NamingConventions conv = new ConditionalConventions(from, to, this.ConventionDirection);
			conventions(conv);

			this.aConditionalConventionList.Add(conv);

			return this;
		}

		#region Overrides of Conventions

		public override IEnumerable<NamingConventions> GetChildConventions()
		{
			foreach (NamingConventions convention in this.aConditionalConventionList)
				yield return convention;
		}

		public override bool CanPair(IMember first, IMember second)
		{
			return true;
		}

		#endregion
	}
}
