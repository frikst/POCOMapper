using KST.POCOMapper.Conventions;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Validation;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class ChangeMemberProcesingOrderWithAutoProps
	{
		private class From
		{
			public string Data = "hello world";
		}

		private class ToAuto
		{
			public string Data { get; set; }
		}

		private class ToCode
		{
			private string aData;

			public string Data
			{
				get { return this.aData; }
				set { this.aData = value; }
			}
		}

		private class MappingAuto : MappingSingleton<MappingAuto>
		{
			private MappingAuto()
			{
				ToConventions
					.SetMemberScanningPrecedence(MemberType.AutoProperty);
				Map<From, ToAuto>();
				Map<From, ToCode>();
			}
		}

		private class MappingCode : MappingSingleton<MappingCode>
		{
			private MappingCode()
			{
				ToConventions
					.SetMemberScanningPrecedence(MemberType.CodeProperty);
				Map<From, ToAuto>();
				Map<From, ToCode>();
			}
		}

		private class MappingProperty : MappingSingleton<MappingProperty>
		{
			private MappingProperty()
			{
				ToConventions
					.SetMemberScanningPrecedence(MemberType.Property);
				Map<From, ToAuto>();
				Map<From, ToCode>();
			}
		}

		[Test]
		public void AutoMapping()
		{
			ToAuto retAuto = MappingAuto.Instance.Map<From, ToAuto>(new From());
			ToCode retCode = MappingAuto.Instance.Map<From, ToCode>(new From());
			Assert.AreEqual("hello world", retAuto.Data);
			Assert.IsNull(retCode.Data);
		}

		[Test]
		public void CodeMapping()
		{
			ToAuto retAuto = MappingCode.Instance.Map<From, ToAuto>(new From());
			ToCode retCode = MappingCode.Instance.Map<From, ToCode>(new From());
			Assert.IsNull(retAuto.Data);
			Assert.AreEqual("hello world", retCode.Data);
		}

		[Test]
		public void PropertyMapping()
		{
			ToAuto retAuto = MappingProperty.Instance.Map<From, ToAuto>(new From());
			ToCode retCode = MappingProperty.Instance.Map<From, ToCode>(new From());
			Assert.AreEqual("hello world", retAuto.Data);
			Assert.AreEqual("hello world", retCode.Data);
		}

		[Test]
		public void ValidateMapping()
		{
			MappingAuto.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
			MappingCode.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
			MappingProperty.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
		}
	}
}
