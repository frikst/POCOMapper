using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Decorators;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class PostprocessRulesDefinition
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
				Map<From, To>()
					.PostprocessRules()
					.Postprocess((a, b) => b.Value = "Hi");
			}
		}

		[Test]
		public void TestPostprocessMap()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
			Assert.AreEqual("Hi", ret.Value);
		}

		[Test]
		public void TestPostprocessSynchronize()
		{
			To ret = new To();
			Mapping.Instance.Synchronize(new From(), ref ret);
			Assert.AreEqual("Hi", ret.Value);
		}
	}
}
