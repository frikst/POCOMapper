using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Collection;
using KST.POCOMapper.Mapping.Decorators;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Mapping.SubClass;
using KST.POCOMapper.Members;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Validation
{
	public class MappingValidationVisitor : IMappingVisitor
	{
		private readonly HashSet<IMapping> aProcessed;

		public MappingValidationVisitor()
		{
			this.aProcessed = new HashSet<IMapping>();
		}

		#region Implementation of IMappingVisitor

		public void Begin()
		{
		}

		public void Next()
		{
		}

		public void End()
		{
		}

		public void Visit(ICollectionMapping mapping)
		{
			if (this.WasProcessed(mapping))
				return;

			mapping.ItemMapping.Accept(this);
		}

		public void Visit(IObjectMapping mapping)
		{
			if (this.WasProcessed(mapping))
				return;

			var shouldBeMappedFrom = this.GetMembersWithAttribute<ShouldBeMappedAttribute>(mapping.From);
			var shouldBeMappedTo = this.GetMembersWithAttribute<ShouldBeMappedAttribute>(mapping.To);

			var shouldNotBeMappedFrom = this.GetMembersWithAttribute<ShouldNotBeMappedAttribute>(mapping.From);
			var shouldNotBeMappedTo = this.GetMembersWithAttribute<ShouldNotBeMappedAttribute>(mapping.To);

			foreach (var memberMapping in mapping.Members)
			{
				var fromMember = this.GetMemberInfo(memberMapping.From, true);
				var toMember = this.GetMemberInfo(memberMapping.To, false);

				if (shouldNotBeMappedFrom.Contains(fromMember))
					throw new MappingValidationException($"{mapping.From.Name}.{fromMember.Name} should not be mapped (mapping to {mapping.To.Name}.{toMember.Name} found)");

				if (shouldNotBeMappedTo.Contains(toMember))
					throw new MappingValidationException($"{mapping.To.Name}.{toMember.Name} should not be mapped (mapping from {mapping.From.Name}.{fromMember.Name} found)");

				if (shouldBeMappedFrom.Contains(fromMember))
					shouldBeMappedFrom.Remove(fromMember);

				if (shouldBeMappedTo.Contains(toMember))
					shouldBeMappedTo.Remove(toMember);
			}

			if (shouldBeMappedFrom.Any())
				throw new MappingValidationException($"{mapping.From.Name}.{shouldBeMappedFrom.First().Name} should be mapped, but no mapping found");

			if (shouldBeMappedTo.Any())
				throw new MappingValidationException($"{mapping.To.Name}.{shouldBeMappedTo.First().Name} should be mapped, but no mapping found");
		}

		public void Visit(ISubClassMapping mapping)
		{
			if (this.WasProcessed(mapping))
				return;

			foreach (var conversion in mapping.Conversions)
			{
				conversion.Mapping.Accept(this);
			}
		}

		public void Visit(IDecoratorMapping mapping)
		{
			if (this.WasProcessed(mapping))
				return;

			mapping.DecoratedMapping.Accept(this);
		}

		public void Visit(IMapping mapping)
		{
		}

		#endregion

		private bool WasProcessed(IMapping mapping)
		{
			return !this.aProcessed.Add(mapping);
		}

		private ISet<MemberInfo> GetMembersWithAttribute<TAttribute>(Type type)
			where TAttribute : Attribute
		{
			return new HashSet<MemberInfo>(
				type.GetMembers().Where(x => x.GetCustomAttributes(typeof(TAttribute), false).Any())
			);
		}

		private MemberInfo GetMemberInfo(IMember member, bool readAccess)
		{
			switch (member)
			{
				case PropertyMember property:
					return property.Property;
				case MethodMember method when readAccess:
					return method.GetMethod;
				case MethodMember method:
					return method.SetMethod;
				case FieldMember field:
					return field.Field;
				default:
					return null;
			}
		}
	}
}
