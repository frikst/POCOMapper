using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
	public class CompositeStructure
	{
		private class From
		{
			public From child;
		}

		private class To
		{
			public To child;
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
			}
		}

		[TestMethod]
		public void CompositeMapping()
		{
			From from = new From { child = new From { child = new From() } };

			To to = Mapping.Instance.Map<From, To>(from);
			Assert.IsNotNull(to.child);
			Assert.IsNotNull(to.child.child);
			Assert.IsNull(to.child.child.child);
		}

		[TestMethod]
		public void CompositeSynchronization()
		{
			From from = new From { child = new From { child = new From() } };
			To to = new To();

			Mapping.Instance.Synchronize(from, ref to);
			Assert.IsNotNull(to.child);
			Assert.IsNotNull(to.child.child);
			Assert.IsNull(to.child.child.child);
		}

		[TestMethod]
		public void CompositeSynchronizationWithNonNullDestinationField()
		{
			From from = new From { child = new From { child = new From() } };
			To to = new To { child = new To() };
			To toChild = to.child;

			Mapping.Instance.Synchronize(from, ref to);
			Assert.IsNotNull(to.child);
			Assert.IsNotNull(to.child.child);
			Assert.IsNull(to.child.child.child);
			Assert.AreSame(toChild, to.child);
		}
	}
}
