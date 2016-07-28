using System.Collections.Generic;
using POCOMapper.conventions.members;
using POCOMapper.mapping.@base;

namespace POCOMapper.mapping.common
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
