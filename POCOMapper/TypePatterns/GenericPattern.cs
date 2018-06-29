using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KST.POCOMapper.TypePatterns
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

		public bool Matches(Type type, TypeChecker typeChecker)
		{
			if (this.aSubclass)
			{
				Type current = type;

				while (current != null)
				{
					if (this.CompareTypeToPattern(current, typeChecker))
						return true;

					foreach (Type @interface in current.GetInterfaces())
					{
						if (this.CompareTypeToPattern(@interface, typeChecker))
							return true;
					}

					current = current.BaseType;
				}
			}
			else
			{
				if (this.CompareTypeToPattern(type, typeChecker))
					return true;
			}

			return false;
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

		private bool CompareTypeToPattern(Type type, TypeChecker typeChecker)
		{
			if (type.IsArray && type.GetArrayRank() == 1)
				type = typeof(IEnumerable<>).MakeGenericType(type.GetElementType());

			if (!type.IsGenericType)
				return false;

			if (!this.aGenericType.Matches(type.GetGenericTypeDefinition(), typeChecker))
				return false;

			Type[] genericParameters = type.GetGenericArguments();

			if (genericParameters.Length != this.aGenericParameters.Count)
				return false;

			return this.aGenericParameters
				.Zip(genericParameters, (paramPattern, paramType) => paramPattern.Matches(paramType, typeChecker))
				.All(x => x);
		}
	}
}
