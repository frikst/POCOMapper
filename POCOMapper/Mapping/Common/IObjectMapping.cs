using System.Collections.Generic;
using KST.POCOMapper.conventions.members;
using KST.POCOMapper.mapping.@base;

namespace KST.POCOMapper.mapping.common
{
	public interface IObjectMemberMapping
	{
		IMember From { get; }
		IMember To { get; }

		IMapping Mapping { get; }
	}

	public interface IObjectMapping : IMapping
	{
		IEnumerable<IObjectMemberMapping> Members { get; }
	}
}
