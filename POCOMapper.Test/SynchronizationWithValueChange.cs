using System;
using KST.POCOMapper.definition;
using KST.POCOMapper.mapping.@base;
using KST.POCOMapper.mapping.standard;
using KST.POCOMapper.visitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.POCOMapper.Test
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
