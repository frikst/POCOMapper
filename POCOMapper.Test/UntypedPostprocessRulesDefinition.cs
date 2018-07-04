using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Decorators;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class UntypedPostprocessRulesDefinition
	{
		private class From
		{
			
		}

		private class To
		{
			public string Value { get; set; }
		}

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map(typeof(From), typeof(To))
					.PostprocessRules()
					.Postprocess((a, b) => b.Value = "Hi");
			}
		}

		[Test]
		public void TestUntypedPostprocessMap()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
			Assert.AreEqual("Hi", ret.Value);
		}

		[Test]
		public void TestUntypedPostprocessSynchronize()
		{
			To ret = new To();
			Mapping.Instance.Synchronize(new From(), ref ret);
			Assert.AreEqual("Hi", ret.Value);
		}
	}
}
