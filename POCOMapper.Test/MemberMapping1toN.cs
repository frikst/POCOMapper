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
	public class MemberMapping1toN
	{
		private class From
		{
			public int i;
		}

		private class To
		{
			public bool i1;
			public bool i2;
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>()
					.ObjectMappingRules()
					.MemberTo<bool>("i1", def => def
						.Using(from => (from.i & 1) == 1)
					)
					.MemberTo<bool>("i2", def => def
						.Using(from => (from.i & 2) == 2)
					);
			}
		}

		[TestMethod]
		public void Mappping0Test()
		{
			From from = new From { i = 0 };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsFalse(to.i1);
			Assert.IsFalse(to.i2);
		}

		[TestMethod]
		public void Mappping1Test()
		{
			From from = new From { i = 1 };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsTrue(to.i1);
			Assert.IsFalse(to.i2);
		}

		[TestMethod]
		public void Mappping2Test()
		{
			From from = new From { i = 2 };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsFalse(to.i1);
			Assert.IsTrue(to.i2);
		}

		[TestMethod]
		public void Mappping3Test()
		{
			From from = new From { i = 3 };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsTrue(to.i1);
			Assert.IsTrue(to.i2);
		}

		[TestMethod]
		public void Synchronization0Test()
		{
			From from = new From { i = 0 };
			To to = new To();

			Mapping.Instance.Synchronize(from, to);
			Assert.IsFalse(to.i1);
			Assert.IsFalse(to.i2);
		}

		[TestMethod]
		public void Synchronization1Test()
		{
			From from = new From { i = 1 };
			To to = new To();

			Mapping.Instance.Synchronize(from, to);
			Assert.IsTrue(to.i1);
			Assert.IsFalse(to.i2);
		}

		[TestMethod]
		public void Synchronization2Test()
		{
			From from = new From { i = 2 };
			To to = new To();

			Mapping.Instance.Synchronize(from, to);
			Assert.IsFalse(to.i1);
			Assert.IsTrue(to.i2);
		}

		[TestMethod]
		public void Synchronization3Test()
		{
			From from = new From { i = 3 };
			To to = new To();

			Mapping.Instance.Synchronize(from, to);
			Assert.IsTrue(to.i1);
			Assert.IsTrue(to.i2);
		}
	}
}
