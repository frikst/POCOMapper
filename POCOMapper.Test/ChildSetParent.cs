using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
	public class ChildSetParent
	{
		private class FromInner
		{

		}

		private class From
		{
			public FromInner Inner = new FromInner();
		}

		private class ToInner
		{
			public To Parent { get; set; }
		}

		private class To
		{
			public ToInner Inner;
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
				Map<FromInner, ToInner>();

				Child<To, ToInner>()
					.Postprocess((parent, child) => child.Parent = parent);
			}
		}

		[TestMethod]
		public void InnerParentIsParentMap()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
			Assert.AreSame(ret, ret.Inner.Parent);
		}

		[TestMethod]
		public void InnerParentIsParentSync()
		{
			To ret = new To();
			Mapping.Instance.Synchronize(new From(), ret);
			Assert.AreSame(ret, ret.Inner.Parent);
		}
	}
}
