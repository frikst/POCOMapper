using System;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Collection
{
	public interface ICollectionMapping : IMapping
	{
		Type ItemFrom { get; }
		Type ItemTo { get; }

		IMapping ItemMapping { get; }
	}
}
