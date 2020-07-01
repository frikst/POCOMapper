using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Mapping.Collection;
using KST.POCOMapper.Validation;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	public class NullToCollection
	{
		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<int[], int[]>()
					.CollectionMappingRules()
					.MapNullToEmpty();
				Map<int[], List<int>>()
					.CollectionMappingRules()
					.MapNullToEmpty();
				Map<int[], HashSet<int>>()
					.CollectionMappingRules()
					.MapNullToEmpty();
				Map<int[], IEnumerable<int>>()
					.CollectionMappingRules()
					.MapNullToEmpty();
			}
		}

		[Test]
		public void ToArray()
		{
			int[] ret = Mapping.Instance.Map<int[], int[]>(new int[] {1, 2, 3});
			Assert.AreEqual(ret.Length, 3);
			Assert.AreEqual(ret[0], 1);
			Assert.AreEqual(ret[1], 2);
			Assert.AreEqual(ret[2], 3);
		}

		[Test]
		public void ToList()
		{
			List<int> ret = Mapping.Instance.Map<int[], List<int>>(new int[] { 1, 2, 3 });
			Assert.AreEqual(ret.Count, 3);
			Assert.AreEqual(ret[0], 1);
			Assert.AreEqual(ret[1], 2);
			Assert.AreEqual(ret[2], 3);
		}

		[Test]
		public void ToHashSet()
		{
			HashSet<int> ret = Mapping.Instance.Map<int[], HashSet<int>>(new int[] { 1, 2, 3 });
			Assert.AreEqual(ret.Count, 3);
			Assert.IsTrue(ret.Contains(1));
			Assert.IsTrue(ret.Contains(2));
			Assert.IsTrue(ret.Contains(3));
		}

		[Test]
		public void ToIEnumerable()
		{
			IEnumerable<int> ret = Mapping.Instance.Map<int[], IEnumerable<int>>(new int[] { 1, 2, 3 });
			Assert.AreEqual(ret.Count(), 3);
			Assert.IsTrue(ret.Contains(1));
			Assert.IsTrue(ret.Contains(2));
			Assert.IsTrue(ret.Contains(3));
		}

		[Test]
		public void NullToArray()
		{
			int[] ret = Mapping.Instance.Map<int[], int[]>(null);
			Assert.NotNull(ret);
			Assert.IsEmpty(ret);
		}

		[Test]
		public void NullToList()
		{
			List<int> ret = Mapping.Instance.Map<int[], List<int>>(null);
			Assert.NotNull(ret);
			Assert.IsEmpty(ret);
		}

		[Test]
		public void NullToHashSet()
		{
			HashSet<int> ret = Mapping.Instance.Map<int[], HashSet<int>>(null);
			Assert.NotNull(ret);
			Assert.IsEmpty(ret);
		}

		[Test]
		public void NullToIEnumerable()
		{
			IEnumerable<int> ret = Mapping.Instance.Map<int[], IEnumerable<int>>(null);
			Assert.NotNull(ret);
			Assert.IsEmpty(ret);
		}
	}
}
