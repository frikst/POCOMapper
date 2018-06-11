using System.Collections.Generic;
using KST.POCOMapper.Conventions.Members;
using KST.POCOMapper.Mapping.Base;

namespace KST.POCOMapper.Mapping.Common
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
