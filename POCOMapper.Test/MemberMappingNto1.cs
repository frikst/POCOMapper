using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;
using POCOMapper.mapping.common;
using POCOMapper.mapping.special;

namespace POCOMapper.Test
{
	[TestClass]
	public class MemberMappingNto1
	{
		private class From
		{
			public bool i1;
			public bool i2;
		}

		private class To
		{
			public int i;
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.ObjectMappingRules()
					.MemberTo<int>("i", def => def
						.FuncMappingRules()
						.Using(from => (from.i1 ? 1 : 0) | (from.i2 ? 2 : 0))
					);
			}
		}

		[TestMethod]
		public void Mappping0Test()
		{
			From from = new From { i1 = false, i2 = false };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual(0, to.i);
		}

		[TestMethod]
		public void Mappping1Test()
		{
			From from = new From { i1 = true, i2 = false };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual(1, to.i);
		}

		[TestMethod]
		public void Mappping2Test()
		{
			From from = new From { i1 = false, i2 = true };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual(2, to.i);
		}

		[TestMethod]
		public void Mappping3Test()
		{
			From from = new From { i1 = true, i2 = true };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.AreEqual(3, to.i);
		}

		[TestMethod]
		public void Synchronization0Test()
		{
			From from = new From { i1 = false, i2 = false };
			To to = new To();

			Mapping.Instance.Synchronize(from, to);
			Assert.AreEqual(0, to.i);
		}

		[TestMethod]
		public void Synchronization1Test()
		{
			From from = new From { i1 = true, i2 = false };
			To to = new To();

			Mapping.Instance.Synchronize(from, to);
			Assert.AreEqual(1, to.i);
		}

		[TestMethod]
		public void Synchronization2Test()
		{
			From from = new From { i1 = false, i2 = true };
			To to = new To();

			Mapping.Instance.Synchronize(from, to);
			Assert.AreEqual(2, to.i);
		}

		[TestMethod]
		public void Synchronization3Test()
		{
			From from = new From { i1 = true, i2 = true };
			To to = new To();

			Mapping.Instance.Synchronize(from, to);
			Assert.AreEqual(3, to.i);
		}
	}
}
