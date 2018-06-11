using System;
using System.Collections;
using System.Collections.Generic;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.POCOMapper.Test
{
	[TestClass]
	public class InvalidConstructorMapping
	{
		private class From
		{
		}

		private class To
		{
			public To(Int32 cislo)
			{

			}
		}

		private class ToCollection<T> : IEnumerable<T>
		{
			public ToCollection(Int32 cislo)
			{

			}

			#region Implementation of IEnumerable

			public IEnumerator<T> GetEnumerator()
			{
				throw new NotImplementedException();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			#endregion
		}

		private class Mapping : MappingDefinition<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
			}
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidMapping))]
		public void TestInvalidObjectConstructor()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidMapping))]
		public void TestInvalidCollectionConstructor()
		{
			ToCollection<Int32> ret = Mapping.Instance.Map<Int32[], ToCollection<Int32>>(new Int32[] { }); 
		}
	}
}
