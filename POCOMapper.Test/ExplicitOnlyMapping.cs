using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
	public class ExplicitOnlyMapping
	{
		private class From
		{
			public string s = "s";
			public string a = "a";
		}

		private class To
		{
			public string helloWorld;
			public string a;
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.OnlyExplicit
					.Member<string, string>("s", "helloWorld", def => { });
			}
		}

		[TestMethod]
		public void MapppingTest()
		{
			From from = new From();

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual("s", to.helloWorld);
			Assert.IsNull(to.a);
		}

		[TestMethod]
		public void SynchronizationTest()
		{
			From from = new From();
			To to = new To();

			Mapping.Instance.Synchronize(from, to);
			Assert.AreEqual("s", to.helloWorld);
			Assert.IsNull(to.a);
		}
	}
}
