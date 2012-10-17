using System;
using System.Linq;

namespace POCOMapper.definition.patterns
{
	public class Pattern<TPattern> : IPattern
	{
		private IPattern aPattern;

		public Pattern()
		{
			this.aPattern = this.Parse(typeof(TPattern), false);
		}

		private IPattern Parse(Type type, bool subclass)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SubClass<>))
				return this.Parse(type.GetGenericArguments()[0], true);
			else if (type.IsGenericType && !type.IsGenericTypeDefinition)
				return new GenericPattern(this.Parse(type.GetGenericTypeDefinition(), subclass), type.GetGenericArguments().Select(x => this.Parse(x, false)), subclass);
			else if (type.IsArray)
				return new ArrayPattern(this.Parse(type.GetElementType(), subclass), type.GetArrayRank());
			else if (type == typeof(T))
				return new AnyPattern();
			else
				return new ExactPattern(type, subclass);
		}

		#region Implementation of IPattern

		public bool Matches(Type type)
		{
			return this.aPattern.Matches(type);
		}

		#endregion
	}
}
