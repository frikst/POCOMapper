using System;
using System.Collections.Generic;
using System.Reflection;
using KST.POCOMapper.Conventions;
using KST.POCOMapper.Definition.ChildProcessingDefinition;
using KST.POCOMapper.Definition.TypeMappingDefinition;
using KST.POCOMapper.Mapping.Collection;
using KST.POCOMapper.Mapping.Standard;
using KST.POCOMapper.TypePatterns;

namespace KST.POCOMapper.Definition
{
	/// <summary>
	/// Generic singleton container of one set of mappings.
	/// </summary>
	/// <typeparam name="TMapping">Descendant class</typeparam>
	public abstract class MappingDefinition<TMapping>
		where TMapping : MappingDefinition<TMapping>
	{
		private static MappingImplementation aMapping;

		private readonly List<ITypeMappingDefinition> aMappingDefinitions;
		private bool aFinished;
		private readonly List<IChildAssociationPostprocessing> aChildPostprocessings;

		protected MappingDefinition()
		{
			this.aMappingDefinitions = new List<ITypeMappingDefinition>();
			this.aChildPostprocessings = new List<IChildAssociationPostprocessing>();
			this.aFinished = false;

			this.FromConventions = new GlobalConventions(NamingConventions.Direction.From);
			this.ToConventions = new GlobalConventions(NamingConventions.Direction.To);

			this.DefaultMappings();
		}

		protected virtual void DefaultMappings()
		{
			this.Map<int, double>()
				.NotVisitable
				.SetPriority(int.MaxValue)
				.CastRules();
			this.Map<double, int>()
				.NotVisitable
				.SetPriority(int.MaxValue)
				.CastRules();

			this.Map<int, string>()
				.NotVisitable
				.SetPriority(int.MaxValue)
				.ToStringRules();
			this.Map<string, int>()
				.NotVisitable
				.SetPriority(int.MaxValue)
				.ParseRules();

			this.Map<string, string>()
				.NotVisitable
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
		protected GlobalConventions FromConventions { get; }

		/// <summary>
		/// Conventions for the destination model.
		/// </summary>
		protected GlobalConventions ToConventions { get; }

		/// <summary>
		/// Defines the mapping of one instance of the class TFrom onto the instance of the class TTo.
		/// </summary>
		/// <typeparam name="TFrom">Class from the source model.</typeparam>
		/// <typeparam name="TTo">Class from the destination model.</typeparam>
		/// <returns>Mapping specification object. Can be used to specify special properties of the mapping.</returns>
		protected ExactTypeMappingDefinition<TFrom, TTo> Map<TFrom, TTo>()
		{
			if (this.aFinished)
				throw new Exception("Cannot modify the mapping");

			ExactTypeMappingDefinition<TFrom, TTo> mappingDefinitionDef = new ExactTypeMappingDefinition<TFrom, TTo>();
			this.aMappingDefinitions.Add(mappingDefinitionDef);
			return mappingDefinitionDef;
		}

		/// <summary>
		/// Defines the mapping of one instance of the class from onto the instance of the class to.
		/// </summary>
		/// <param name="from">Class from the source model.</param>
		/// <param name="to">Class from the destination model.</param>
		/// <returns>Mapping specification object. Can be used to specify special properties of the mapping.</returns>
		protected UntypedTypeMappingDefinition Map(Type from, Type to)
		{
			if (this.aFinished)
				throw new Exception("Cannot modify the mapping");

			UntypedTypeMappingDefinition mappingDefinitionDef = new UntypedTypeMappingDefinition(from, to);
			this.aMappingDefinitions.Add(mappingDefinitionDef);
			return mappingDefinitionDef;
		}

		/// <summary>
		/// Defines the mapping of one instance of the container TFrom onto the instance of the container TTo.
		/// <see cref="T"/> should be used as the collection item type.
		/// </summary>
		/// <param name="patternFrom">Pattern for class from the source model.</param>
		/// <param name="patternTo">Pattern for class from the destination model.</param>
		/// <returns>Mapping specification object. Can be used to specify special properties of the mapping.</returns>
		protected PatternTypeMappingDefinition Map(IPattern patternFrom, IPattern patternTo)
		{
			if (this.aFinished)
				throw new Exception("Cannot modify the mapping");

			PatternTypeMappingDefinition mappingDefinitionDef = new PatternTypeMappingDefinition(patternFrom, patternTo);
			this.aMappingDefinitions.Add(mappingDefinitionDef);
			return mappingDefinitionDef;
		}

		protected ChildAssociationPostprocessing<TParent, TChild> Child<TParent, TChild>()
		{
			if (this.aFinished)
				throw new Exception("Cannot modify the mapping");

			ChildAssociationPostprocessing<TParent, TChild> mappingDefinitionDef = new ChildAssociationPostprocessing<TParent, TChild>();
			this.aChildPostprocessings.Add(mappingDefinitionDef);
			return mappingDefinitionDef;
		}

		/// <summary>
		/// Instance of the singleton. Should be used only on the <see cref="MappingDefinition{TMapping}"/> descendant.
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
