using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;
using POCOMapper.mapping.@base;
using POCOMapper.mapping.standard;
using POCOMapper.visitor;

namespace POCOMapper.Test
{
	[TestClass]
	public class SynchronizationWithValueChange
	{
		private class From
		{
			public int child;
		}

		private class To
		{
			public int child;
		}

		private class TestMapping : IMapping<int, int>
		{
			#region Implementation of IMapping

			public void Accept(IMappingVisitor visitor)
			{
				visitor.Visit(this);
			}

			public bool CanSynchronize { get { return true; } }
			public bool CanMap { get { return true; } }
			public bool IsDirect { get { return false; } }
			public bool SynchronizeCanChangeObject { get { return true; } }
			public string MappingSource { get { return null; } }
			public string SynchronizationSource { get { return null; } }
			public Type From { get { return typeof(int); } }
			public Type To { get { return typeof(int); } }

			#endregion

			#region Implementation of IMapping<From,To>

			public int Map(int @from)
			{
				return 5;
			}

			public int Synchronize(int @from, int to)
			{
				return 5;
			}

			#endregion
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
				Map<int, int>()
					.CustomMappingRules()
					.MappingFactory(x => new TestMapping());
			}
		}

		[TestMethod]
		public void MappingTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
			Assert.AreEqual(5, ret.child);
		}

		[TestMethod]
		public void SynchronizationTest()
		{
			To to = new To();
			Mapping.Instance.Synchronize(new From(), ref to);
			Assert.AreEqual(5, to.child);
		}
	}
}
