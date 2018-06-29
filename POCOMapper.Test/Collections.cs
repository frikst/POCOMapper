using System;
using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Exceptions;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class Collections
	{
		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				
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
		public void ToArrayWithCast()
		{
			double[] ret = Mapping.Instance.Map<int[], double[]>(new int[] {1, 2, 3});
			Assert.AreEqual(ret.Length, 3);
			Assert.AreEqual(ret[0], 1d);
			Assert.AreEqual(ret[1], 2d);
			Assert.AreEqual(ret[2], 3d);
		}

		[Test]
		public void ToArrayWithInvalidCast()
		{
			Assert.Throws<UnknownMappingException>(delegate
			{
				int[] ret = Mapping.Instance.Map<double[], int[]>(new double[] {1, 2, 3});
			});
		}
	}
}
