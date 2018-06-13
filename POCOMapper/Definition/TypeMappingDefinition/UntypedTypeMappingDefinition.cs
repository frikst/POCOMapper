using System;
using System.Reflection;
using KST.POCOMapper.Internal.ReflectionMembers;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Object;

namespace KST.POCOMapper.Definition.TypeMappingDefinition
{
	/// <summary>
	/// Untyped mapping specification definition class.
	/// </summary>
	public class UntypedTypeMappingDefinition : IExactTypeMappingDefinition, IRulesDefinition
	{
		private readonly Type aFrom;
		private readonly Type aTo;

		private int aPriority;
		private IMappingRules aRules;

		internal UntypedTypeMappingDefinition(Type from, Type to)
		{
			this.aFrom = from;
			this.aTo = to;
			this.aRules = new ObjectMappingRules();
		}

		#region Implementation of IMappingDefinition

		IMapping ITypeMappingDefinition.CreateMapping(MappingImplementation allMappings, Type from, Type to)
		{
			MethodInfo mappingCreateMethod = MappingRulesMethods.GetCreate(from, to);
			return (IMapping)mappingCreateMethod.Invoke(this.aRules, new object[] { allMappings });
		}

		bool ITypeMappingDefinition.IsFrom(Type from)
		{
			return this.aFrom == from;
		}

		bool ITypeMappingDefinition.IsTo(Type to)
		{
			return this.aTo == to;
		}

		int ITypeMappingDefinition.Priority
			=> this.aPriority;

		#endregion

		public UntypedTypeMappingDefinition SetPriority(int priority)
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

		#region Implementation of IExactMappingDefinition

		Type IExactTypeMappingDefinition.From
			=> this.aFrom;

		Type IExactTypeMappingDefinition.To
			=> this.aTo;

		#endregion
	}
}
