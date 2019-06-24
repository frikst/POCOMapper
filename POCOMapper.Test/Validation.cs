using KST.POCOMapper.Definition;
using KST.POCOMapper.Validation;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class Validation
	{
		private class From
		{
			public int Field1;
		}

		private class ValidTo
		{
			[ShouldBeMapped]
			public int Field1;

			public int Field2;
		}

		private class InvalidTo
		{
			public int Field1;

			[ShouldBeMapped]
			public int Field2;
		}

		private class ValidToShouldNot
		{
			public int Field1;

			[ShouldNotBeMapped]
			public int Field2;
		}

		private class InvalidToShouldNot
		{
			[ShouldNotBeMapped]
			public int Field1;

			public int Field2;
		}

        private class ValidToBasePrivateSet
        {
            [ShouldBeMapped]
            public int Field1 { get; private set; }
        }

        private class ValidToInheritedPrivateSet : ValidToBasePrivateSet
        {
        }

		private class ValidMapping : MappingSingleton<ValidMapping>
		{
			protected ValidMapping()
			{
				this.Map<From, ValidTo>();
			}
		}

		private class InvalidMapping : MappingSingleton<InvalidMapping>
		{
			protected InvalidMapping()
			{
				this.Map<From, InvalidTo>();
			}
		}

		private class ValidMappingShouldNot : MappingSingleton<ValidMappingShouldNot>
		{
			protected ValidMappingShouldNot()
			{
				this.Map<From, ValidToShouldNot>();
			}
		}

		private class InvalidMappingShouldNot : MappingSingleton<InvalidMappingShouldNot>
		{
			protected InvalidMappingShouldNot()
			{
				this.Map<From, InvalidToShouldNot>();
			}
		}

		private class ValidMappingInheritedPrivateSet : MappingSingleton<ValidMappingInheritedPrivateSet>
		{
			protected ValidMappingInheritedPrivateSet()
			{
				this.Map<From, ValidToInheritedPrivateSet>();
			}
		}

		[Test]
		public void ValidateValidMapping()
		{
			ValidMapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}

		[Test]
		public void ValidateInvalidMapping()
		{
			Assert.Throws<MappingValidationException>(() => InvalidMapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor()));
		}

		[Test]
		public void ValidateValidMappingShouldNot()
		{
			ValidMappingShouldNot.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}

		[Test]
		public void ValidateInvalidMappingShouldNot()
		{
			Assert.Throws<MappingValidationException>(() => InvalidMappingShouldNot.Instance.Mappings.AcceptForAll(new MappingValidationVisitor()));
		}

        [Test]
        public void ValidateValidMappingInheritedPrivateSet()
        {
            ValidMappingInheritedPrivateSet.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
        }
	}
}
