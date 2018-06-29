using System;
using System.Collections.Generic;
using KST.POCOMapper.Conventions.MemberParsers;
using KST.POCOMapper.Conventions.SymbolConventions;
using KST.POCOMapper.Executor;
using KST.POCOMapper.Members;

namespace KST.POCOMapper.Conventions
{
	public abstract class NamingConventions
	{
		public enum Direction
		{
			From,
			To
		}

		internal NamingConventions(Direction conventionDirection, ISymbolConvention fields, ISymbolConvention methods, ISymbolConvention properties, IEnumerable<MemberType> memberScanningPrecedence)
		{
			this.ConventionDirection = conventionDirection;

			this.Fields = fields;
			this.Methods = methods;
			this.Properties = properties;

			this.MemberScanningPrecedence = memberScanningPrecedence;
		}

		internal void SetMappingDefinition(MappingDefinitionInformation mappingDefinition)
		{
			this.MappingDefinition = mappingDefinition;

			foreach (var namingConventions in this.GetChildConventions())
				namingConventions.SetMappingDefinition(mappingDefinition);
		}

		protected MappingDefinitionInformation MappingDefinition { get; private set; }

		public ISymbolConvention Fields { get; }
		public ISymbolConvention Methods { get; }
		public ISymbolConvention Properties { get; }

		public IEnumerable<IMember> GetAllMembers(Type type, IMember parent = null)
			=> new MemberIterator(type, this, parent);

		public IEnumerable<MemberType> MemberScanningPrecedence { get; }

		public Direction ConventionDirection { get; }

		public abstract IEnumerable<NamingConventions> GetChildConventions();
		public abstract bool CanPair(IMember first, IMember second);
	}
}
