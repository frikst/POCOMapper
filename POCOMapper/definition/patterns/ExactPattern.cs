using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMapper.definition.patterns
{
	internal class ExactPattern : IPattern
	{
		private Type aType;
		private readonly bool aSubclass;

		public ExactPattern(Type type, bool subclass)
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
