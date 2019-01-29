using System;
using System.Collections;
using System.Collections.Generic;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Validation;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
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

		private class Mapping : MappingSingleton<Mapping>
		{
			private Mapping()
			{
				Map<From, To>();
			}
		}

		[Test]
		public void TestInvalidObjectConstructor()
		{
			Assert.Throws<InvalidMappingException>(() => Mapping.Instance.Map<From, To>(new From()));
		}

		[Test]
		public void TestInvalidCollectionConstructor()
		{
			Assert.Throws<InvalidMappingException>(() => Mapping.Instance.Map<Int32[], ToCollection<Int32>>(new Int32[] { }));
		}

		[Test]
		public void ValidateMapping()
		{
			Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
