using System;
using System.Reflection;
using KST.POCOMapper.Definition.ChildProcessingDefinition;
using KST.POCOMapper.Definition.Conventions;
using KST.POCOMapper.Definition.TypeMappingDefinition;
using KST.POCOMapper.Executor;
using KST.POCOMapper.TypePatterns;
using KST.POCOMapper.TypePatterns.DefinitionHelpers;

namespace KST.POCOMapper.Definition
{
	/// <summary>
	/// Generic singleton container of one set of mappings.
	/// </summary>
	/// <typeparam name="TMapping">Descendant class</typeparam>
	public abstract class MappingSingleton<TMapping>
		where TMapping : MappingSingleton<TMapping>
	{
		private static MappingExecutor aMapping;

		private readonly MappingBuilder aBuilder;

		protected MappingSingleton()
		{
			this.aBuilder = new MappingBuilder();
		}

		/// <summary>
		/// Conventions for the source model.
		/// </summary>
		protected GlobalNamingConventionsBuilder FromConventions
			=> this.aBuilder.FromConventions;

		/// <summary>
		/// Conventions for the destination model.
		/// </summary>
		protected GlobalNamingConventionsBuilder ToConventions
			=> this.aBuilder.ToConventions;

		/// <summary>
		/// Defines the mapping of one instance of the class TFrom onto the instance of the class TTo.
		/// </summary>
		/// <typeparam name="TFrom">Class from the source model.</typeparam>
		/// <typeparam name="TTo">Class from the destination model.</typeparam>
		/// <returns>Mapping specification object. Can be used to specify special properties of the mapping.</returns>
		protected ExactTypeMappingDefinition<TFrom, TTo> Map<TFrom, TTo>()
			=> this.aBuilder.Map<TFrom, TTo>();

		/// <summary>
		/// Defines the mapping of one instance of the class from onto the instance of the class to.
		/// </summary>
		/// <param name="from">Class from the source model.</param>
		/// <param name="to">Class from the destination model.</param>
		/// <returns>Mapping specification object. Can be used to specify special properties of the mapping.</returns>
		protected UntypedTypeMappingDefinition Map(Type from, Type to)
			=> this.aBuilder.Map(from, to);

		/// <summary>
		/// Defines the mapping of one instance of the container TFrom onto the instance of the container TTo.
		/// <see cref="T"/> should be used as the collection item type.
		/// </summary>
		/// <param name="patternFrom">Pattern for class from the source model.</param>
		/// <param name="patternTo">Pattern for class from the destination model.</param>
		/// <returns>Mapping specification object. Can be used to specify special properties of the mapping.</returns>
		protected PatternTypeMappingDefinition Map(IPattern patternFrom, IPattern patternTo)
			=> this.aBuilder.Map(patternFrom, patternTo);

		protected ChildAssociationPostprocessing<TParent, TChild> Child<TParent, TChild>()
			=> this.aBuilder.Child<TParent, TChild>();

		/// <summary>
		/// Instance of the singleton. Should be used only on the <see cref="MappingSingleton{TMapping}"/> descendant.
		/// </summary>
		public static MappingExecutor Instance
		{
			get
			{
				if (aMapping == null)
				{
					var ci = typeof(TMapping).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { }, null);
					var definition = (MappingSingleton<TMapping>) ci.Invoke(null);

					aMapping = definition.aBuilder.Finish();
				}

				return aMapping;
			}
		}
	}
}
