using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMapper.conventions;
using POCOMapper.exceptions;
using POCOMapper.mapping.@base;

namespace POCOMapper.definition
{
	public class MappingImplementation
	{
		private readonly Dictionary<Tuple<Type, Type>, IMappingDefinition> aClassMappingDefinitions;
		private readonly Dictionary<Tuple<Type, Type>, IMappingDefinition> aContainerMappingDefinitions;
		private readonly Dictionary<Tuple<Type, Type>, IMapping> aMappings;

		internal MappingImplementation(IEnumerable<IMappingDefinition> mappingDefinitions, Conventions fromConventions, Conventions toConventions)
		{
			this.aMappings = new Dictionary<Tuple<Type, Type>, IMapping>();
			this.aClassMappingDefinitions = new Dictionary<Tuple<Type, Type>, IMappingDefinition>();
			aContainerMappingDefinitions = new Dictionary<Tuple<Type, Type>, IMappingDefinition>();

			foreach (IMappingDefinition def in mappingDefinitions)
			{
				if (def.Type == MappingType.ClassMapping)
					this.aClassMappingDefinitions[new Tuple<Type, Type>(def.From, def.To)] = def;
				else
					this.aContainerMappingDefinitions[new Tuple<Type, Type>(def.From, def.To)] = def;
			}


			this.FromConventions = fromConventions;
			this.ToConventions = toConventions;
		}

		internal Conventions FromConventions { get; private set; }
		internal Conventions ToConventions { get; private set; }

		public IMapping GetMapping(Type from, Type to)
		{
			Tuple<Type, Type> key = new Tuple<Type, Type>(from, to);

			if (this.aMappings.ContainsKey(key))
				return this.aMappings[key];
			
			if (this.aClassMappingDefinitions.ContainsKey(key))
			{
				IMapping mapping = this.aClassMappingDefinitions[key].CreateMapping(this, from, to);
				this.aMappings[key] = mapping;
				return mapping;
			}

			bool fromIsEnumerable = from.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
			bool toIsEnumerable = to.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));

			Type fromType = from.IsArray ? typeof(T[]) : from.GetGenericTypeDefinition().MakeGenericType(typeof(T));
			Type toType = to.IsArray ? typeof(T[]) : to.GetGenericTypeDefinition().MakeGenericType(typeof(T));

			if (fromIsEnumerable && toIsEnumerable)
			{
				List<Type> fromPosibilities = new List<Type>();
				for (Type fromBase = fromType; fromBase != typeof (object); fromBase = fromBase.BaseType)
					if ((typeof(IEnumerable<T>).IsAssignableFrom(fromBase)))
						fromPosibilities.Add(fromBase);
				fromPosibilities.AddRange(fromType.GetInterfaces().Where(x => typeof(IEnumerable<T>).IsAssignableFrom(x)));

				List<Type> toPosibilities = new List<Type>();
				for (Type toBase = toType; toBase != typeof(object); toBase = toBase.BaseType)
					if ((typeof(IEnumerable<T>).IsAssignableFrom(toBase)))
						toPosibilities.Add(toBase);
				toPosibilities.AddRange(toType.GetInterfaces().Where(x => typeof(IEnumerable<T>).IsAssignableFrom(x)));

				foreach (Type fromBase in fromPosibilities)
				{
					foreach (Type toBase in toPosibilities)
					{
						Tuple<Type, Type> baseKey = new Tuple<Type, Type>(fromBase, toBase);

						if (aContainerMappingDefinitions.ContainsKey(baseKey))
						{
							IMapping mapping = this.aContainerMappingDefinitions[baseKey].CreateMapping(this, from, to);
							this.aMappings[key] = mapping;
							return mapping;
						}
					}
				}
			}

			return null;
		}

		public IMapping<TFrom, TTo> GetMapping<TFrom, TTo>()
		{
			return (IMapping<TFrom, TTo>)GetMapping(typeof(TFrom), typeof(TTo));
		}

		public TTo Map<TFrom, TTo>(TFrom from)
		{
			IMapping<TFrom, TTo> mapping = this.GetMapping<TFrom, TTo>();

			if (mapping == null)
				throw new UnknownMapping(typeof(TFrom), typeof(TTo));

			if (mapping == null)
				throw new Exception(string.Format("Unknown mapping from type {0} to type {1}", typeof(TFrom).Name, typeof(TTo).Name));

			return mapping.Map(from);
		}

		public void Synchronize<TFrom, TTo>(TFrom from, TTo to)
		{
			IMapping<TFrom, TTo> mapping = this.GetMapping<TFrom, TTo>();

			if (mapping == null)
				throw new UnknownMapping(typeof(TFrom), typeof(TTo));

			if (mapping == null)
				throw new Exception(string.Format("Unknown mapping from type {0} to type {1}", typeof(TFrom).Name, typeof(TTo).Name));

			mapping.Synchronize(from, to);
		}

		private void MappingToString(IMapping mapping, StringBuilder output, int level)
		{
			string indent = string.Concat(Enumerable.Range(0, level).Select(x => "    "));

			if (mapping == null)
			{
				output.Append("(null)\n");
			}
			else
			{
				Type mappingType = mapping.GetType();
				output.Append(mappingType.Name);

				if (mappingType.IsGenericType)
				{
					bool begining = true;
					output.Append("<");

					foreach (Type genericArgument in mappingType.GetGenericArguments())
					{
						if (!begining)
							output.Append(", ");
						output.Append(genericArgument.Name);
						begining = false;
					}

					output.Append(">");
				}

				output.Append("\n");

				foreach (Tuple<string, IMapping> child in mapping.Children)
				{
					output.Append(indent);
					output.Append(child.Item1);
					output.Append(" ");

					this.MappingToString(child.Item2, output, level + 1);
				}
			}
		}

		public string MappingToString<TFrom, TTo>()
		{
			StringBuilder output = new StringBuilder();

			this.MappingToString(this.GetMapping<TFrom, TTo>(), output, 1);

			return output.ToString();
		}
	}
}
