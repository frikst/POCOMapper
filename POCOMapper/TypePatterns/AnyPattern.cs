using System;

namespace KST.POCOMapper.TypePatterns
{
	internal class AnyPattern : IPattern
	{
		private readonly Type aPlaceholderType;

		public AnyPattern(Type placeholderType)
		{
			this.aPlaceholderType = placeholderType;
		}

		public AnyPattern()
		{
			this.aPlaceholderType = null;
		}

		#region Implementation of IPattern

		public bool Matches(Type type, TypeChecker typeChecker)
		{
			if (this.aPlaceholderType == null)
				return true;

			return typeChecker.Check(this.aPlaceholderType, type);
		}

		public override string ToString()
		{
			return "?";
		}

		#endregion
	}
}
