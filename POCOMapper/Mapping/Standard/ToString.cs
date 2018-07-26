using System;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Visitor;

namespace KST.POCOMapper.Mapping.Standard
{
	public class ToString<TFrom> : IMapping<TFrom, string>
	{
		public void Accept(IMappingVisitor visitor)
		{
			visitor.Visit(this);
		}

		public Type From
			=> typeof(TFrom);

		public Type To
			=> typeof(string);

		public string Map(TFrom from)
		{
			return from.ToString();
		}
	}
}
