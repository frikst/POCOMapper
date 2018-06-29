using System;
using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Conventions;
using KST.POCOMapper.Definition.ChildProcessingDefinition;
using KST.POCOMapper.Definition.Conventions;
using KST.POCOMapper.Definition.TypeMappingDefinition;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Internal;
using KST.POCOMapper.Mapping.Collection;
using KST.POCOMapper.Mapping.Standard;
using KST.POCOMapper.TypePatterns;
using KST.POCOMapper.TypePatterns.DefinitionHelpers;

namespace KST.POCOMapper.Definition
{
	public class MappingBuilder
	{
		private abstract class GItemFrom : GenericParameter { }
		private abstract class GItemTo : GenericParameter { }

		private readonly List<ITypeMappingDefinition> aMappingDefinitions;
		private readonly List<IChildAssociationPostprocessing> aChildPostprocessings;
		private bool aFinished;

		public MappingBuilder()
		{
			this.aMappingDefinitions = new List<ITypeMappingDefinition>();
			this.aChildPostprocessings = new List<IChildAssociationPostprocessing>();
			this.aFinished = false;

			this.FromConventions = new GlobalNamingConventionsBuilder(NamingConventions.Direction.From);
			this.ToConventions = new GlobalNamingConventionsBuilder(NamingConventions.Direction.To);

			this.DefaultMappings();
		}

		private void DefaultMappings()
		{
			this.Map(new Pattern<GItemFrom>(), new Pattern<GItemFrom>())
				.Where(x => x.IsPrimitiveOrPrimitiveLike<GItemFrom>())
				.SetPriority(int.MaxValue)
				.CopyRules();

			this.Map(new Pattern<GItemFrom>(), new Pattern<string>())
				.Where(x => x.Type<GItemFrom>() != typeof(string))
				.Where(x => x.IsPrimitiveOrPrimitiveLike<GItemFrom>())
				.SetPriority(int.MaxValue)
				.ToStringRules();

			this.Map(new Pattern<GItemFrom>(), new Pattern<GItemTo>())
				.Where(x => x.IsImplicitlyCastable<GItemFrom, GItemTo>())
				.SetPriority(int.MaxValue)
				.CastRules();

			this.Map(new Pattern<SubClass<IEnumerable<GItemFrom>>>(), new Pattern<GItemTo[]>())
				.Where(x => x.Mappable<GItemFrom, GItemTo>())
				.SetPriority(int.MaxValue)
				.CollectionMappingRules()
				.MapAs(CollectionMappingType.Array);
			this.Map(new Pattern<SubClass<IEnumerable<GItemFrom>>>(), new Pattern<List<GItemTo>>())
				.Where(x => x.Mappable<GItemFrom, GItemTo>())
				.SetPriority(int.MaxValue)
				.CollectionMappingRules()
				.MapAs(CollectionMappingType.List);
			this.Map(new Pattern<SubClass<IEnumerable<GItemFrom>>>(), new Pattern<SubClass<IEnumerable<GItemTo>>>())
				.Where(x => x.Mappable<GItemFrom, GItemTo>())
				.SetPriority(int.MaxValue)
				.CollectionMappingRules();
		}

		/// <summary>
		/// Conventions for the source model.
		/// </summary>
		public GlobalNamingConventionsBuilder FromConventions { get; }

		/// <summary>
		/// Conventions for the destination model.
		/// </summary>
		public GlobalNamingConventionsBuilder ToConventions { get; }

		/// <summary>
		/// Defines the mapping of one instance of the class TFrom onto the instance of the class TTo.
		/// </summary>
		/// <typeparam name="TFrom">Class from the source model.</typeparam>
		/// <typeparam name="TTo">Class from the destination model.</typeparam>
		/// <returns>Mapping specification object. Can be used to specify special properties of the mapping.</returns>
		public ExactTypeMappingDefinition<TFrom, TTo> Map<TFrom, TTo>()
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
		public UntypedTypeMappingDefinition Map(Type from, Type to)
		{
			if (this.aFinished)
				throw new Exception("Cannot modify the mapping");

			UntypedTypeMappingDefinition mappingDefinitionDef = new UntypedTypeMappingDefinition(from, to);
			this.aMappingDefinitions.Add(mappingDefinitionDef);
			return mappingDefinitionDef;
		}

		/// <summary>
		/// Defines the mapping of one instance of the type defined by pattern patternFrom onto the instance of the type defined
		/// by pattern patternTo.
		/// </summary>
		/// <param name="patternFrom">Pattern for class from the source model.</param>
		/// <param name="patternTo">Pattern for class from the destination model.</param>
		/// <returns>Mapping specification object. Can be used to specify special properties of the mapping.</returns>
		public PatternTypeMappingDefinition Map(IPattern patternFrom, IPattern patternTo)
		{
			if (this.aFinished)
				throw new Exception("Cannot modify the mapping");

			PatternTypeMappingDefinition mappingDefinitionDef = new PatternTypeMappingDefinition(patternFrom, patternTo);
			this.aMappingDefinitions.Add(mappingDefinitionDef);
			return mappingDefinitionDef;
		}

		public ChildAssociationPostprocessing<TParent, TChild> Child<TParent, TChild>()
		{
			if (this.aFinished)
				throw new Exception("Cannot modify the mapping");

			ChildAssociationPostprocessing<TParent, TChild> mappingDefinitionDef = new ChildAssociationPostprocessing<TParent, TChild>();
			this.aChildPostprocessings.Add(mappingDefinitionDef);
			return mappingDefinitionDef;
		}

		public MappingExecutor Finish()
		{
			this.aFinished = true;
			return new MappingExecutor(
				this.aMappingDefinitions,
				this.aChildPostprocessings,
				this.FromConventions.Finish(),
				this.ToConventions.Finish()
			);
		}
	}
}
