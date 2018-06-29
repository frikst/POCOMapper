using System;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Internal;
using KST.POCOMapper.TypePatterns.DefinitionHelpers;

namespace KST.POCOMapper.TypePatterns.Group
{
	public class PatternWhereEvaluator
	{
		private readonly TypeChecker aTypeChecker;
		private readonly MappingDefinitionInformation aMappingDefinitionInformation;

		internal PatternWhereEvaluator(TypeChecker typeChecker, MappingDefinitionInformation mappingDefinitionInformation)
		{
			this.aTypeChecker = typeChecker;
			this.aMappingDefinitionInformation = mappingDefinitionInformation;
		}

		private Type Resolve<TPlaceholderOrType>()
		{
			if (typeof(GenericParameter).IsAssignableFrom(typeof(TPlaceholderOrType)))
				return this.aTypeChecker.GetType<TPlaceholderOrType>();

			return typeof(TPlaceholderOrType);
		}

		public Type Type<TPlaceholder>() where TPlaceholder : GenericParameter
			=> this.aTypeChecker.GetType<TPlaceholder>();

		public string Name<TPlaceholder>() where TPlaceholder : GenericParameter
			=> this.Type<TPlaceholder>().Name;

		public bool IsClass<TPlaceholder>() where TPlaceholder : GenericParameter
			=> !this.Type<TPlaceholder>().IsValueType;

		public bool IsStruct<TPlaceholder>() where TPlaceholder : GenericParameter
			=> this.Type<TPlaceholder>().IsValueType;

		public bool IsEnum<TPlaceholder>() where TPlaceholder : GenericParameter
			=> this.Type<TPlaceholder>().IsEnum;

		public bool IsDelegate<TPlaceholder>() where TPlaceholder : GenericParameter
			=> this.Type<TPlaceholder>().IsSubclassOf(typeof(Delegate));

		public bool HasNew<TPlaceholder>() where TPlaceholder : GenericParameter
			=> this.Type<TPlaceholder>().GetConstructor(new Type[0]) != null;

		public bool Is<TPlaceholder, TCheckedType>() where TPlaceholder : GenericParameter
			=> this.Resolve<TCheckedType>().IsAssignableFrom(this.Type<TPlaceholder>());

		public bool InAssemblyOf<TPlaceholder, TCheckedType>() where TPlaceholder : GenericParameter
			=> this.Resolve<TCheckedType>().Assembly == this.Type<TPlaceholder>().Assembly;

		public bool IsPrimitive<TPlaceholder>() where TPlaceholder : GenericParameter
			=> BasicNetTypes.IsPrimitiveType(this.Type<TPlaceholder>());

		public bool IsPrimitiveOrPrimitiveLike<TPlaceholder>() where TPlaceholder : GenericParameter
			=> BasicNetTypes.IsPrimitiveOrPrimitiveLikeType(this.Type<TPlaceholder>());

		public bool IsImplicitlyCastable<TTypeFrom, TTypeTo>()
			=> BasicNetTypes.IsImplicitlyCastable(this.Resolve<TTypeFrom>(), this.Resolve<TTypeTo>());

		public bool IsExplicitlyCastable<TTypeFrom, TTypeTo>()
			=> BasicNetTypes.IsExplicitlyCastable(this.Resolve<TTypeFrom>(), this.Resolve<TTypeTo>());

		public bool Mappable<TTypeFrom, TTypeTo>()
			=> this.aMappingDefinitionInformation.HasMapping(this.Resolve<TTypeFrom>(), this.Resolve<TTypeTo>());
	}
}
