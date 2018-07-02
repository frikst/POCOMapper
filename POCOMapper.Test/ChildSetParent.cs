using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Special;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
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

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
				Map<FromInner, ToInner>()
					.NullableRules();

				Child<To, ToInner>()
					.Postprocess((parent, child) => child.Parent = parent);
			}
		}

		[Test]
		public void InnerParentIsParentMap()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
			Assert.AreSame(ret, ret.Inner.Parent);
		}

		[Test]
		public void InnerParentIsParentSync()
		{
			To ret = new To();
			Mapping.Instance.Synchronize(new From(), ref ret);
			Assert.AreSame(ret, ret.Inner.Parent);
		}
	}
}
