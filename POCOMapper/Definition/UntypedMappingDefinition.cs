using System;
using System.Reflection;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.common;

namespace POCOMapper.definition
{
	/// <summary>
	/// Untyped mapping specification definition class.
	/// </summary>
	public class UntypedMappingDefinition : IMappingDefinition, IRulesDefinition
	{
		private readonly Type aFrom;
		private readonly Type aTo;

		private int aPriority;
		private IMappingRules aRules;

		internal UntypedMappingDefinition(Type from, Type to)
		{
			this.aFrom = from;
			this.aTo = to;
			this.aRules = new ObjectMappingRules();
		}

		#region Implementation of IMappingDefinition

		IMapping IMappingDefinition.CreateMapping(MappingImplementation allMappings, Type from, Type to)
		{
			MethodInfo mappingCreateMethod = typeof(IMappingRules).GetMethod("Create").MakeGenericMethod(from, to);
			return (IMapping)mappingCreateMethod.Invoke(this.aRules, new object[] { allMappings });
		}

		bool IMappingDefinition.IsFrom(Type from)
		{
			return this.aFrom == from;
		}

		bool IMappingDefinition.IsTo(Type to)
		{
			return this.aTo == to;
		}

		Tuple<Type, Type> IMappingDefinition.GetKey()
		{
			return new Tuple<Type, Type>(this.aFrom, this.aTo);
		}

		int IMappingDefinition.Priority
		{
			get { return this.aPriority; }
		}

		#endregion

		public UntypedMappingDefinition SetPriority(int priority)
		{
			this.aPriority = priority;

			return this;
		}

		#region Implementation of IRulesDefinition

		public TRules Rules<TRules>()
			where TRules : class, IMappingRules, new()
		{
			TRules ret = new TRules();
			this.aRules = ret;
			return ret;
		}

		#endregion
	}
}
