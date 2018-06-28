using System;
using System.Reflection;
using KST.POCOMapper.Executor;
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
		private bool aVisitable;

		internal UntypedTypeMappingDefinition(Type from, Type to)
		{
			this.aFrom = from;
			this.aTo = to;
			this.aVisitable = true;
			this.aPriority = 0;
			this.aRules = new ObjectMappingRules();
		}

		#region Implementation of IMappingDefinition

		IMapping ITypeMappingDefinition.CreateMapping(MappingDefinitionInformation mappingDefinition, Type from, Type to)
		{
			if (this.aFrom != from || this.aTo != to)
				throw new InvalidOperationException($"{from.Name} and {to.Name} does not match required types");

			MethodInfo mappingCreateMethod = MappingRulesMethods.GetCreate(from, to);
			return (IMapping)mappingCreateMethod.Invoke(this.aRules, new object[] { mappingDefinition });
		}

		bool ITypeMappingDefinition.IsDefinedFor(Type from, Type to)
		{
			return this.aFrom == from && this.aTo == to;
		}

		int ITypeMappingDefinition.Priority
			=> this.aPriority;

		bool ITypeMappingDefinition.Visitable
			=> this.aVisitable;

		#endregion

		public UntypedTypeMappingDefinition SetPriority(int priority)
		{
			this.aPriority = priority;

			return this;
		}

		public UntypedTypeMappingDefinition NotVisitable
		{
			get
			{
				this.aVisitable = false;

				return this;
			}
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
