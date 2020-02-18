using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Collection;
using KST.POCOMapper.Mapping.Decorators;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Mapping.SubClass;
using KST.POCOMapper.Members;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Validation
{
	public class MappingErrorsLoggingVisitor : IMappingVisitor
	{
		private readonly HashSet<IMapping> aProcessed;
		private readonly LogErrorInMappingDelegate aLogErrorInMapping;
		private readonly ValidationDefaultHandling aDefaultHandling;

		public MappingErrorsLoggingVisitor(LogErrorInMappingDelegate logErrorInMapping, ValidationDefaultHandling defaultHandling = ValidationDefaultHandling.Default)
		{
			this.aProcessed = new HashSet<IMapping>();
			this.aLogErrorInMapping = logErrorInMapping;
			this.aDefaultHandling = defaultHandling;
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

			ISet<MemberInfo> shouldBeMappedFrom;
			ISet<MemberInfo> shouldBeMappedTo;
			ISet<MemberInfo> shouldNotBeMappedFrom;
			ISet<MemberInfo> shouldNotBeMappedTo;

			if (this.aDefaultHandling == ValidationDefaultHandling.ShouldNotBeMapped)
			{
				shouldNotBeMappedFrom = this.GetMembersWithoutAttribute<ShouldBeMappedAttribute>(mapping.From);
				shouldNotBeMappedTo = this.GetMembersWithoutAttribute<ShouldBeMappedAttribute>(mapping.To);
			}
			else
			{
				shouldNotBeMappedFrom = this.GetMembersWithAttribute<ShouldNotBeMappedAttribute>(mapping.From);
				shouldNotBeMappedTo = this.GetMembersWithAttribute<ShouldNotBeMappedAttribute>(mapping.To);
			}

			if (this.aDefaultHandling == ValidationDefaultHandling.ShouldBeMapped)
			{
				shouldBeMappedFrom = this.GetMembersWithoutAttribute<ShouldNotBeMappedAttribute>(mapping.From);
				shouldBeMappedTo = this.GetMembersWithoutAttribute<ShouldNotBeMappedAttribute>(mapping.To);
			}
			else
			{
				shouldBeMappedFrom = this.GetMembersWithAttribute<ShouldBeMappedAttribute>(mapping.From);
				shouldBeMappedTo = this.GetMembersWithAttribute<ShouldBeMappedAttribute>(mapping.To);
			}

			foreach (var memberMapping in mapping.Members)
			{
				var fromMember = this.GetMemberInfo(memberMapping.From, true);
				var toMember = this.GetMemberInfo(memberMapping.To, false);

				if (shouldNotBeMappedFrom.Contains(fromMember))
					this.aLogErrorInMapping(new ValidationError.ExtraMappingFrom(mapping.From, fromMember, mapping.To, toMember));

				if (shouldNotBeMappedTo.Contains(toMember))
					this.aLogErrorInMapping(new ValidationError.ExtraMappingTo(mapping.From, fromMember, mapping.To, toMember));

				if (shouldBeMappedFrom.Contains(fromMember))
					shouldBeMappedFrom.Remove(fromMember);

				if (shouldBeMappedTo.Contains(toMember))
					shouldBeMappedTo.Remove(toMember);
			}

			foreach (var member in shouldBeMappedFrom)
				this.aLogErrorInMapping(new ValidationError.MissingMappingFrom(mapping.From, member, mapping.To));
			foreach (var member in shouldBeMappedTo)
				this.aLogErrorInMapping(new ValidationError.MissingMappingTo(mapping.From, mapping.To, member));
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
            var members = new HashSet<MemberInfo>();

            for (Type current = type; current != null; current = current.BaseType)
            {
                members.UnionWith(
                    current.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
	                    .Where(x => !x.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any())
	                    .Where(x => !(x is MethodInfo xmi && xmi.IsSpecialName) && !(x is ConstructorInfo))
                        .Where(x => x.GetCustomAttributes(typeof(TAttribute), false).Any())
                );
            }

            return members;
        }

		private ISet<MemberInfo> GetMembersWithoutAttribute<TAttribute>(Type type)
			where TAttribute : Attribute
		{
            var members = new HashSet<MemberInfo>();

            for (Type current = type; current != null; current = current.BaseType)
            {
                members.UnionWith(
                    current.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
	                    .Where(x => !x.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any())
	                    .Where(x => !(x is MethodInfo xmi && xmi.IsSpecialName) && !(x is ConstructorInfo))
                        .Where(x => !x.GetCustomAttributes(typeof(TAttribute), false).Any())
                );
            }

            return members;
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
