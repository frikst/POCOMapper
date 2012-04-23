using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
	public class CollectionChildSetParent
	{
		private class From
		{

		}

		private class To
		{
			public IEnumerable Parent { get; set; }
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();

				Child<IEnumerable, To>()
					.Postprocess((parent, child) => child.Parent = parent);
			}
		}

		[TestMethod]
		public void InnerParentIsParentMap()
		{
			List<To> ret = Mapping.Instance.Map<IEnumerable<From>, List<To>>(new List<From> { new From() });
			Assert.AreSame(ret, ret[0].Parent);
		}
	}
}
