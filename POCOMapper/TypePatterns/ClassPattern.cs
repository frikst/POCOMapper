using System;

namespace POCOMapper.typePatterns
{
	internal class ClassPattern : IPattern
	{
		private Type aType;
		private readonly bool aSubclass;

		public ClassPattern(Type type, bool subclass)
		{
			this.aType = type;
			this.aSubclass = subclass;
		}

		#region Implementation of IPattern

		public bool Matches(Type type)
		{
			if (this.aSubclass)
				return this.IsBase(type, this.aType);
			else
				return type == this.aType;
		}

		public override string ToString()
		{
			if (this.aSubclass)
			{
				if (this.aType.IsInterface)
					return "? implements " + this.aType.FullName;
				else
					return "? extends " + this.aType.FullName;
			}
			else
				return this.aType.FullName;
		}

		#endregion

		public bool IsBase(Type unknown, Type @base)
		{
			if (unknown.IsGenericType && !unknown.IsGenericTypeDefinition)
				unknown = unknown.GetGenericTypeDefinition();

			if (@base == unknown)
				return true;

			if (unknown.BaseType != null && this.IsBase(unknown.BaseType, @base))
				return true;

			foreach (Type @interface in unknown.GetInterfaces())
			{
				if (this.IsBase(@interface, @base))
					return true;
			}

			return false;
		}
	}
}
