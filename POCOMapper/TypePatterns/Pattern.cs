using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KST.POCOMapper.Exceptions;

namespace KST.POCOMapper.TypePatterns
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
				return new GenericPattern(this.Parse(type.GetGenericTypeDefinition(), false), type.GetGenericArguments().Select(x => this.Parse(x, false)), subclass);
			else if (type.IsArray)
				return new ArrayPattern(this.Parse(type.GetElementType(), subclass), type.GetArrayRank());
			else if (type == typeof(T))
				return new AnyPattern();
			else
				return new ClassPattern(type, subclass);
		}

		#region Implementation of IPattern

		public bool Matches(Type type)
		{
			return this.aPattern.Matches(type);
		}

		public override string ToString()
		{
			return this.aPattern.ToString();
		}

		#endregion
	}

	public class Pattern : IPattern
	{
		private enum State
		{
			Begin,
			Implements,
			Extends
		}

		private static readonly string TOKEN_EMPTY = "";
		private static readonly string TOKEN_ANY = "?";
		private static readonly string TOKEN_IMPLEMENTS = "?implements";
		private static readonly string TOKEN_EXTENDS = "?extends";
		private static readonly string TOKEN_GENERIC_BEGIN = "<";
		private static readonly string TOKEN_GENERIC_SEPARATOR = ",";
		private static readonly string TOKEN_GENERIC_END = ">";

		private IPattern aPattern;
		private Assembly aAssembly;

		public Pattern(Assembly assembly, string pattern)
		{
			this.aAssembly = assembly;
			int index = 0;
			this.aPattern = this.Parse(pattern, ref index);
			if (index != pattern.Length)
				throw new InvalidPatternException(pattern, index);
		}

		private IPattern Parse(string pattern, ref int indexFrom)
		{
			string token = TOKEN_EMPTY;
			State state = State.Begin;

			for (int i = indexFrom; i < pattern.Length; i++)
			{
				char current = pattern[i];

				if (char.IsWhiteSpace(current))
					continue;

				if (state == State.Begin && current == TOKEN_ANY[0] && token == "")
					token = "?";
				else if (TOKEN_IMPLEMENTS.StartsWith((token + current)))
				{
					token += current;
					if (token == TOKEN_IMPLEMENTS)
					{
						state = State.Implements;
						token = TOKEN_EMPTY;
					}
				}
				else if (TOKEN_EXTENDS.StartsWith((token + current)))
				{
					token += current;
					if (token == TOKEN_EXTENDS)
					{
						state = State.Extends;
						token = TOKEN_EMPTY;
					}
				}
				else if (token == TOKEN_ANY)
				{
					indexFrom = i;
					return new AnyPattern();
				}
				else if ((token == TOKEN_EMPTY || token.All(this.IsClassNameChar)) && this.IsClassNameChar(current))
				{
					token += current;
				}
				else if (token != TOKEN_EMPTY && token.All(this.IsClassNameChar) && current == TOKEN_GENERIC_BEGIN[0])
				{
					i++;
					List<IPattern> parameters = new List<IPattern>();

					while (true)
					{
						parameters.Add(this.Parse(pattern, ref i));

						while (char.IsWhiteSpace(pattern[i]))
							i++;

						char c = pattern[i];

						i++;

						if (c == TOKEN_GENERIC_END[0])
							break;
						else if (c != TOKEN_GENERIC_SEPARATOR[0])
							throw new InvalidPatternException(pattern, pattern.Length);
					}

					Type generic = this.aAssembly.GetType(token + "`" + parameters.Count);

					if (generic == null)
						throw new InvalidPatternException(pattern, indexFrom, "Unknown type");

					if (!generic.IsGenericType)
						throw new InvalidPatternException(pattern, indexFrom, "The given type is not generic type");

					if (state == State.Extends && !generic.IsClass)
						throw new InvalidPatternException(pattern, indexFrom, "The given type is not class type");

					if (state == State.Implements && !generic.IsInterface)
						throw new InvalidPatternException(pattern, indexFrom, "The given type is not interface");


					indexFrom = i;
					return new GenericPattern(new ClassPattern(generic, false), parameters, state == State.Implements || state == State.Extends);
				}
				else if (token != TOKEN_EMPTY && token.All(this.IsClassNameChar) && this.IsClassNameChar(current))
				{
					token += current;
				}
				else if (token != TOKEN_EMPTY && token.All(this.IsClassNameChar))
				{
					Type @class = this.aAssembly.GetType(token);

					if (@class == null)
						throw new InvalidPatternException(pattern, indexFrom, "Unknown type");

					if (@class.IsGenericType)
						throw new InvalidPatternException(pattern, indexFrom, "The given type is not generic type");

					if (state == State.Extends && !@class.IsClass)
						throw new InvalidPatternException(pattern, indexFrom, "The given type is not class type");

					if (state == State.Implements && !@class.IsInterface)
						throw new InvalidPatternException(pattern, indexFrom, "The given type is not interface");

					indexFrom = i;
					return new ClassPattern(@class, state == State.Implements || state == State.Extends);
				}
				else
					throw new InvalidPatternException(pattern, pattern.Length);
			}

			if (token == TOKEN_ANY)
			{
				indexFrom = pattern.Length;
				return new AnyPattern();
			}

			if (token != TOKEN_EMPTY && token.All(this.IsClassNameChar))
			{
				Type @class = this.aAssembly.GetType(token);

				if (@class == null)
					throw new InvalidPatternException(pattern, indexFrom, "Unknown type");

				if (@class.IsGenericType)
					throw new InvalidPatternException(pattern, indexFrom, "The given type is not generic type");

				if (state == State.Extends && !@class.IsClass)
					throw new InvalidPatternException(pattern, indexFrom, "The given type is not class type");

				if (state == State.Implements && !@class.IsInterface)
					throw new InvalidPatternException(pattern, indexFrom, "The given type is not interface");

				indexFrom = pattern.Length;
				return new ClassPattern(@class, state == State.Implements || state == State.Extends);
			}

			throw new InvalidPatternException(pattern, pattern.Length, "Unexpected end of pattern");
		}

		private bool IsClassNameChar(char current)
		{
			return char.IsLetterOrDigit(current) || current == '.' || current == '+';
		}

		#region Implementation of IPattern

		public bool Matches(Type type)
		{
			return this.aPattern.Matches(type);
		}

		public override string ToString()
		{
			return this.aPattern.ToString();
		}

		#endregion
	}
}
