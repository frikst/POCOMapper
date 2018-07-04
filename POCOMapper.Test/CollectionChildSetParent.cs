using System.Collections;
using System.Collections.Generic;
using KST.POCOMapper.Definition;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class CollectionChildSetParent
	{
		private class From
		{

		}

		private class To
		{
			public IEnumerable Parent { get; set; }
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();

				Child<IEnumerable, To>()
					.Postprocess((parent, child) => child.Parent = parent);
			}
		}

		[Test]
		public void InnerCollectionParentIsParentMap()
		{
			List<To> ret = Mapping.Instance.Map<IEnumerable<From>, List<To>>(new List<From> { new From() });
			Assert.AreSame(ret, ret[0].Parent);
		}
	}
}
