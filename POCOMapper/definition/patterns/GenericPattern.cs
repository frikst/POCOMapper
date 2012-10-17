using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMapper.definition.patterns
{
	internal class GenericPattern : IPattern
	{
		private readonly IPattern aGenericType;
		private readonly bool aSubclass;
		private readonly List<IPattern> aGenericParameters;

		public GenericPattern(IPattern genericType, IEnumerable<IPattern> genericParameters, bool subclass)
		{
			this.aGenericType = genericType;
			this.aSubclass = subclass;
			this.aGenericParameters = genericParameters.ToList();
		}

		#region Implementation of IPattern

		public bool Matches(Type type)
		{
			if (type.IsArray && type.GetArrayRank() == 1)
				type = typeof (IEnumerable<>).MakeGenericType(type.GetElementType());

			if (this.aSubclass)
			{
				while (!type.IsGenericType)
				{
					if (type.BaseType == null)
						return false;
					type = type.BaseType;
				}
			}

			if (!type.IsGenericType)
				return false;

			if (! this.aGenericType.Matches(type.GetGenericTypeDefinition()))
				return false;

			Type[] genericParameters = type.GetGenericArguments();

			if (genericParameters.Length != this.aGenericParameters.Count)
				return false;

			foreach (Tuple<IPattern, Type> genericParameter in aGenericParameters.Zip(genericParameters, (a, b) => new Tuple<IPattern, Type>(a, b)))
			{
				if (!genericParameter.Item1.Matches(genericParameter.Item2))
					return false;
			}

			return true;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("[");
			sb.Append(this.aGenericType);
			sb.Append("]");
			sb.Append("<");

			bool first = true;

			foreach (IPattern parameter in this.aGenericParameters)
			{
				if (!first)
					sb.Append(", ");

				sb.Append(parameter);

				first = false;
			}

			sb.Append(">");

			return sb.ToString();
		}

		#endregion
	}
}
