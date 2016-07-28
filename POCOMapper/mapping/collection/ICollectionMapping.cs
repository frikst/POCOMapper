using System;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.collection
{
	public interface ICollectionMapping : IMapping
	{
		Type ItemFrom { get; }
		Type ItemTo { get; }

		IMapping ItemMapping { get; }
	}
}
