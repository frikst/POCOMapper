using System;
using System.Collections.Generic;
using KST.POCOMapper.Exceptions;

namespace KST.POCOMapper.TypePatterns
{
	public class TypeChecker
	{
		private Dictionary<Type, Type> aTypes;

		public TypeChecker()
		{
			this.aTypes = new Dictionary<Type, Type>();
		}

		public bool Check(Type placeholderType, Type foundType)
		{
			if (this.aTypes.TryGetValue(placeholderType, out var originalType))
				return foundType == originalType;

			this.aTypes.Add(placeholderType, foundType);
			return true;
		}

		public Type GetType<TPlaceholder>()
		{
			if (this.aTypes.TryGetValue(typeof(TPlaceholder), out var foundType))
				return foundType;

			throw new InvalidPlaceholderTypeException(typeof(TPlaceholder));
		}
	}
}