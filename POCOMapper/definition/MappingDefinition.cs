using System;
using System.Collections.Generic;
using System.Reflection;
using POCOMapper.conventions;
using POCOMapper.mapping.collection;
using POCOMapper.mapping.standard;
using POCOMapper.typePatterns;

namespace POCOMapper.definition
{
	/// <summary>
	/// Generic singleton container of one set of mappings.
	/// </summary>
	/// <typeparam name="TMapping">Descendant class</typeparam>
	public abstract class MappingDefinition<TMapping>
		where TMapping : MappingDefinition<TMapping>
	{
		private static MappingImplementation aMapping;

		private readonly List<IMappingDefinition> aMappingDefinitions;
		private bool aFinished;
		private List<IChildAssociationPostprocessing> aChildPostprocessings;

		protected MappingDefinition()
		{
			this.aMappingDefinitions = new List<IMappingDefinition>();
			this.aChildPostprocessings = new List<IChildAssociationPostprocessing>();
			this.aFinished = false;

			this.FromConventions = new GlobalConventions(Conventions.Direction.From);
			this.ToConventions = new GlobalConventions(Conventions.Direction.To);

			this.DefaultMappings();
		}

		protected virtual void DefaultMappings()
		{
			this.Map<int, double>()
				.SetPriority(int.MaxValue)
				.CastRules();;
			this.Map<double, int>()
				.SetPriority(int.MaxValue)
				.CastRules();

			this.Map<int, string>()
				.SetPriority(int.MaxValue)
				.ToStringRules();
			this.Map<string, int>()
				.SetPriority(int.MaxValue)
				.ParseRules();

			this.Map<string, string>()
				.SetPriority(int.MaxValue)
				.CopyRules();

			this.Map(new Pattern<SubClass<IEnumerable<T>>>(), new Pattern<T[]>())
				.SetPriority(int.MaxValue)
				.CollectionMappingRules()
				.MapAs(CollectionMappingType.Array);
			this.Map(new Pattern<SubClass<IEnumerable<T>>>(), new Pattern<List<T>>())
				.SetPriority(int.MaxValue)
				.CollectionMappingRules()
				.MapAs(CollectionMappingType.List);
			this.Map(new Pattern<SubClass<IEnumerable<T>>>(), new Pattern<SubClass<IEnumerable<T>>>())
				.SetPriority(int.MaxValue)
				.CollectionMappingRules();
		}

		/// <summary>
		/// Conventions for the source model.
		/// </summary>
		protected GlobalConventions FromConventions { get; private set; }

		/// <summary>
		/// Conventions for the destination model.
		/// </summary>
		protected GlobalConventions ToConventions { get; private set; }

		/// <summary>
		/// Defines the mapping of one instance of the class TFrom onto the instance of the class TTo.
		/// </summary>
		/// <typeparam name="TFrom">Class from the source model.</typeparam>
		/// <typeparam name="TTo">Class from the destination model.</typeparam>
		/// <returns>Mapping specification object. Can be used to specify special properties of the mapping.</returns>
		protected ExactMappingDefinition<TFrom, TTo> Map<TFrom, TTo>()
		{
			if (aFinished)
				throw new Exception("Cannot modify the mapping");

			ExactMappingDefinition<TFrom, TTo> mappingDefinitionDef = new ExactMappingDefinition<TFrom, TTo>();
			this.aMappingDefinitions.Add(mappingDefinitionDef);
			return mappingDefinitionDef;
		}

		/// <summary>
		/// Defines the mapping of one instance of the container TFrom onto the instance of the container TTo.
		/// <see cref="T"/> should be used as the collection item type.
		/// </summary>
		/// <typeparam name="TFrom">Class from the source model.</typeparam>
		/// <typeparam name="TTo">Class from the destination model.</typeparam>
		/// <returns>Mapping specification object. Can be used to specify special properties of the mapping.</returns>
		protected PatternMappingDefinition Map(IPattern patternFrom, IPattern patternTo)
		{
			if (aFinished)
				throw new Exception("Cannot modify the mapping");

			PatternMappingDefinition mappingDefinitionDef = new PatternMappingDefinition(patternFrom, patternTo);
			this.aMappingDefinitions.Add(mappingDefinitionDef);
			return mappingDefinitionDef;
		}

		protected ChildAssociationPostprocessing<TParent, TChild> Child<TParent, TChild>()
		{
			if (aFinished)
				throw new Exception("Cannot modify the mapping");

			ChildAssociationPostprocessing<TParent, TChild> mappingDefinitionDef = new ChildAssociationPostprocessing<TParent, TChild>();
			this.aChildPostprocessings.Add(mappingDefinitionDef);
			return mappingDefinitionDef;
		}

		/// <summary>
		/// Instance of the singleton. Should be used only on the <see cref="POCOMapper.definition.MappingDefinition{T}"/> descendant.
		/// </summary>
		public static MappingImplementation Instance
		{
			get
			{
				if (aMapping == null)
				{
					Type mapDefType = typeof(TMapping);

					ConstructorInfo ci = mapDefType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { }, null);
					MappingDefinition<TMapping> definition = (MappingDefinition<TMapping>)ci.Invoke(null);
					definition.aFinished = true;

					aMapping = new MappingImplementation(
						definition.aMappingDefinitions,
						definition.aChildPostprocessings,
						definition.FromConventions,
						definition.ToConventions
					);
				}

				return aMapping;
			}
		}
	}
}
