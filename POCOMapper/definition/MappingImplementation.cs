using System;
using System.Collections.Generic;
using System.Linq;
using POCOMapper.conventions;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.collection;

namespace POCOMapper.definition
{
	public class MappingImplementation
	{
		private Dictionary<Tuple<Type, Type>, IMapping> aMappings;
		private Dictionary<Tuple<Type, Type>, SingleMappingDefinition> aMappingDefinitions;

		internal MappingImplementation(IEnumerable<SingleMappingDefinition> mappingDefinitions, Conventions fromConventions, Conventions toConventions)
		{
			this.aMappings = new Dictionary<Tuple<Type, Type>, IMapping>();
			this.aMappingDefinitions = new Dictionary<Tuple<Type, Type>, SingleMappingDefinition>();

			foreach (SingleMappingDefinition def in mappingDefinitions)
				this.aMappingDefinitions[new Tuple<Type, Type>(def.From, def.To)] = def;

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
			
			if (this.aMappingDefinitions.ContainsKey(key))
			{
				IMapping mapping = this.aMappingDefinitions[key].CreateMapping(this);
				this.aMappings[key] = mapping;
				return mapping;
			}

			bool fromIsEnum = from.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
			bool toIsEnum = to.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
			bool toIsArray = to.IsArray;
			bool toIsList = to.IsGenericType && to.GetGenericTypeDefinition() == typeof(List<>);

			if (fromIsEnum && toIsEnum)
			{
				if (toIsArray)
				{
					IMapping mapping = (IMapping)Activator.CreateInstance(typeof(EnumerableToArray<,>).MakeGenericType(from, to), this);
					this.aMappings[key] = mapping;
					return mapping;
				}
				else if (toIsList)
				{
					IMapping mapping = (IMapping)Activator.CreateInstance(typeof(EnumerableToList<,>).MakeGenericType(from, to), this);
					this.aMappings[key] = mapping;
					return mapping;
				}
				else
				{
					IMapping mapping = (IMapping)Activator.CreateInstance(typeof(EnumerableToEnumerable<,>).MakeGenericType(from, to), this);
					this.aMappings[key] = mapping;
					return mapping;
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
				throw new Exception(string.Format("Unknown mapping from type {0} to type {1}", typeof(TFrom).Name, typeof(TTo).Name));

			return mapping.Map(from);
		}
	}
}
