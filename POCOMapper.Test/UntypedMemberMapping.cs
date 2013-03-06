using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;
using POCOMapper.mapping.common;

namespace POCOMapper.Test
{
	[TestClass]
	public class UntypedMemberMapping
	{
		private class From
		{
			public string s = "s";
			public string a = "a";
			public int b = 1;
		}

		private class To
		{
			public string helloWorld;
			public string a;
			public string number;
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.ObjectMappingRules()
					.Member("s", "helloWorld")
					.Member("b", "number");
			}
		}

		[TestMethod]
		public void MapppingTest()
		{
			From from = new From();

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual("s", to.helloWorld);
			Assert.AreEqual("a", to.a);
			Assert.AreEqual("1", to.number);
		}

		[TestMethod]
		public void SynchronizationTest()
		{
			From from = new From();
			To to = new To();

			Mapping.Instance.Synchronize(from, to);
			Assert.AreEqual("s", to.helloWorld);
			Assert.AreEqual("a", to.a);
			Assert.AreEqual("1", to.number);
		}
	}
}
