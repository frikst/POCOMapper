using System;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Object;

namespace KST.POCOMapper.Mapping.Special
{
	public class PostprocessRules<TFrom, TTo> : IMappingRules<TFrom, TTo>, IRulesDefinition<TFrom, TTo>
	{
		private Action<TFrom, TTo> aPostprocessDelegate;
		private IMappingRules<TFrom, TTo> aRules;

		public PostprocessRules()
		{
			this.aPostprocessDelegate = null;
			this.aRules = new ObjectMappingRules<TFrom, TTo>();
		}

		/// <summary>
		/// Adds the entity postprocess delegate. Delegate is called for each entity pair after
		/// the mapping process is complete.
		/// </summary>
		/// <param name="postprocessDelegate">The delegate which should be called after the mapping process.</param>
		/// <returns>The class definition specification object.</returns>
		public PostprocessRules<TFrom, TTo> Postprocess(Action<TFrom, TTo> postprocessDelegate)
		{
			this.aPostprocessDelegate = postprocessDelegate;

			return this;
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

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules<TFrom, TTo>.Create(MappingDefinitionInformation mappingDefinition)
		{
			var mapping = this.aRules.Create(mappingDefinition);
			if (mapping is IMappingWithSyncSupport<TFrom, TTo> mappingWithSync)
				return new PostprocessWithSync<TFrom, TTo>(mappingWithSync, this.aPostprocessDelegate);
			else
				return new PostprocessWithMap<TFrom, TTo>(mapping, this.aPostprocessDelegate);
		}

		#endregion
	}

	public class PostprocessRules : IMappingRules, IRulesDefinition
	{
		private Action<dynamic, dynamic> aPostprocessDelegate;
		private IMappingRules aRules;

		public PostprocessRules()
		{
			this.aPostprocessDelegate = null;
			this.aRules = new ObjectMappingRules();
		}

		/// <summary>
		/// Adds the entity postprocess delegate. Delegate is called for each entity pair after
		/// the mapping process is complete.
		/// </summary>
		/// <param name="postprocessDelegate">The delegate which should be called after the mapping process.</param>
		/// <returns>The class definition specification object.</returns>
		public PostprocessRules Postprocess(Action<dynamic, dynamic> postprocessDelegate)
		{
			this.aPostprocessDelegate = postprocessDelegate;

			return this;
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

		#region Implementation of IMappingRules

		IMapping<TFrom, TTo> IMappingRules.Create<TFrom, TTo>(MappingDefinitionInformation mappingDefinition)
		{
			var mapping = this.aRules.Create<TFrom, TTo>(mappingDefinition);
			if (mapping is IMappingWithSyncSupport<TFrom, TTo> mappingWithSync)
				return new PostprocessWithSync<TFrom, TTo>(mappingWithSync, (a, b) => this.aPostprocessDelegate(a, b));
			else
				return new PostprocessWithMap<TFrom, TTo>(mapping, (a, b) => this.aPostprocessDelegate(a, b));
		}

		#endregion
	}
}
