﻿using System;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Base;
using KST.POCOMapper.Mapping.Standard;
using KST.POCOMapper.Validation;
using KST.POCOMapper.Visitor;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class CustomMappingWithValueChange
	{
		private class From
		{
			public int child;
		}

		private class To
		{
			public int child;
		}

		private class TestMapping : IMappingWithSyncSupport<int, int>
		{
			#region Implementation of IMapping

			public void Accept(IMappingVisitor visitor)
			{
				visitor.Visit(this);
			}

			public bool IsDirect { get { return false; } }
			public bool SynchronizeCanChangeObject { get { return true; } }
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

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
				Map<int, int>()
					.CustomMappingRules()
					.MappingFactory(x => new TestMapping());
			}
		}

		[Test]
		public void CustomMappingTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
			Assert.AreEqual(5, ret.child);
		}

		[Test]
		public void CustomMappingSynchronizationTest()
		{
			To to = new To();
			Mapping.Instance.Synchronize(new From(), ref to);
			Assert.AreEqual(5, to.child);
		}

		[Test]
		public void ValidateMapping()
		{
			Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
