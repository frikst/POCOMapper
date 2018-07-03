using System;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Object;

namespace KST.POCOMapper.Mapping.Decorators
{
	public class NullableRules<TFrom, TTo> : IMappingRules<TFrom, TTo>, IRulesDefinition<TFrom, TTo>
		where TFrom : class
		where TTo : class
	{
		private IMappingRules<TFrom, TTo> aRules;

		public NullableRules()
		{
			this.aRules = new ObjectMappingRules<TFrom, TTo>();
		}

		#region Implementation of IRulesDefinition<TFrom,TTo>

		public TRules Rules<TRules>()
			where TRules : class, IMappingRules<TFrom, TTo>, new()
		{
			TRules ret = new TRules();
			this.aRules = ret;
			return ret;
		}

		#endregion

		#region Implementation of IMappingRules<TFrom,TTo>

		public IMapping<TFrom, TTo> Create(MappingDefinitionInformation mappingDefinition)
		{
			var underlayingMapping = this.aRules.Create(mappingDefinition);

			if (underlayingMapping is IMappingWithSyncSupport<TFrom, TTo> mappingWithSync)
				return new NullableWithSync<TFrom, TTo>(mappingWithSync);
			else
				return new NullableWithMap<TFrom, TTo>(underlayingMapping);
		}

		#endregion
	}

	public class NullableRules : IMappingRules, IRulesDefinition
	{
		private IMappingRules aRules;

		public NullableRules()
		{
			this.aRules = new ObjectMappingRules();
		}

		#region Implementation of IRulesDefinition<TFrom,TTo>

		public TRules Rules<TRules>()
			where TRules : class, IMappingRules, new()
		{
			TRules ret = new TRules();
			this.aRules = ret;
			return ret;
		}

		#endregion

		#region Implementation of IMappingRules<TFrom,TTo>

		public IMapping<TFrom, TTo> Create<TFrom, TTo>(MappingDefinitionInformation mappingDefinition)
		{
			// TODO: support for nullable value types
			if (typeof(TFrom).IsValueType || typeof(TTo).IsValueType)
				throw new InvalidMappingException($"Both source type {typeof(TFrom).Name} and destination type {typeof(TTo).Name} should be reference types");

			var underlayingMapping = this.aRules.Create<TFrom, TTo>(mappingDefinition);

			if (underlayingMapping is IMappingWithSyncSupport<TFrom, TTo> mappingWithSync)
			{
				return (IMapping<TFrom, TTo>) Activator.CreateInstance(
					typeof(NullableWithSync<,>).MakeGenericType(typeof(TFrom), typeof(TTo)),
					mappingWithSync
				);
			}
			else
			{
				return (IMapping<TFrom, TTo>) Activator.CreateInstance(
					typeof(NullableWithMap<,>).MakeGenericType(typeof(TFrom), typeof(TTo)),
					underlayingMapping
				);
			}
		}

		#endregion
	}
}
