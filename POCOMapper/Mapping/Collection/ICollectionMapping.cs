using System;
using KST.POCOMapper.mapping.@base;

namespace KST.POCOMapper.mapping.collection
{
	public interface ICollectionMapping : IMapping
	{
		Type ItemFrom { get; }
		Type ItemTo { get; }

		IMapping ItemMapping { get; }
	}
}
