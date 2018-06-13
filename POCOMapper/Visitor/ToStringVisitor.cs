using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Collection;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Mapping.SubClass;

namespace KST.POCOMapper.Visitor
{
	public class ToStringVisitor : IMappingVisitor
	{
		private static readonly Regex aReNumberGenericArguments = new Regex("`[0-9]+$");

		private readonly StringBuilder aResult;
		private readonly HashSet<Type> aProcessed;
		private int aLevel;
		private bool aFirstLine;
		private string aTypeIdentification;

		public ToStringVisitor()
		{
			this.aResult = new StringBuilder();
			this.aProcessed = new HashSet<Type>();
			this.aLevel = 0;
			this.aFirstLine = true;
			this.aTypeIdentification = null;
		}

		#region Implementation of IMappingVisitor

		public void Begin()
		{
			
		}

		public void Next()
		{
			this.aResult.Append('\n');
			this.aResult.Append("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
		}

		public void End()
		{

		}

		public void Visit(ICollectionMapping mapping)
		{
			this.AddTypeLine(mapping);

			if (this.CheckRecursivelyProcessed(mapping))
				return;

			this.aLevel++;
			this.aTypeIdentification = "[item]";
			if (mapping.ItemMapping != null)
				mapping.ItemMapping.Accept(this);
			this.aLevel--;

			this.aProcessed.Remove(mapping.GetType());
		}

		public void Visit(IObjectMapping mapping)
		{
			this.AddTypeLine(mapping);

			if (this.CheckRecursivelyProcessed(mapping))
				return;

			this.aLevel++;
			foreach (var member in mapping.Members)
			{
				this.aTypeIdentification = $"{member.From.FullName} => {member.To.FullName}";
				if (member.Mapping == null)
					this.AddNullLine();
				else
					member.Mapping.Accept(this);
			}
			this.aLevel--;

			this.aProcessed.Remove(mapping.GetType());
		}

		public void Visit(ISubClassMapping mapping)
		{
			this.AddTypeLine(mapping);

			if (this.CheckRecursivelyProcessed(mapping))
				return;

			this.aLevel++;
			foreach (var conversion in mapping.Conversions)
			{
				this.aTypeIdentification = $"{conversion.From.Name} => {conversion.To.Name}";
				conversion.Mapping.Accept(this);
			}
			this.aLevel--;

			this.aProcessed.Remove(mapping.GetType());
		}

		public void Visit(IMapping mapping)
		{
			this.AddTypeLine(mapping);
		}

		#endregion

		private void EndLine()
		{
			if (!this.aFirstLine)
				this.aResult.Append('\n');
			this.aFirstLine = false;

			this.aResult.Append(string.Concat(Enumerable.Repeat("    ", this.aLevel)));
		}

		private void AddTypeLine(IMapping mapping)
		{
			Type type = mapping.GetType();

			this.EndLine();
			this.AddTypeInformation();

			this.aResult.Append(ToStringVisitor.aReNumberGenericArguments.Replace(type.Name, ""));
			if (type.IsGenericType)
			{
				this.aResult.Append("<");

				bool first = true;
				foreach (var argument in type.GetGenericArguments())
				{
					if (!first)
						this.aResult.Append(", ");
					first = false;

					this.aResult.Append(argument.Name);
				}

				this.aResult.Append(">");
			}
		}

		private void AddNullLine()
		{
			this.EndLine();
			this.AddTypeInformation();

			this.aResult.Append("(null)");
		}

		private bool CheckRecursivelyProcessed(IMapping mapping)
		{
			Type type = mapping.GetType();

			if (this.aProcessed.Contains(type))
			{
				this.aLevel++;
				this.EndLine();
				this.aResult.Append("...");
				this.aLevel--;

				return true;
			}

			this.aProcessed.Add(type);

			return false;
		}

		private void AddTypeInformation()
		{
			if (this.aTypeIdentification != null)
			{
				this.aResult.Append(this.aTypeIdentification);
				this.aResult.Append(" ");
			}
			this.aTypeIdentification = null;
		}

		public string GetResult()
		{
			return this.aResult.ToString();
		}
	}
}