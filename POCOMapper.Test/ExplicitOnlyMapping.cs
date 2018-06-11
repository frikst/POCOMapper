﻿using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Common;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
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
					.ObjectMappingRules()
					.OnlyExplicit
					.Member<string, string>("s", "helloWorld", def => { });
			}
		}

		[Test]
		public void MapppingTest()
		{
			From from = new From();

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual("s", to.helloWorld);
			Assert.IsNull(to.a);
		}

		[Test]
		public void SynchronizationTest()
		{
			From from = new From();
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.AreEqual("s", to.helloWorld);
			Assert.IsNull(to.a);
		}
	}
}
