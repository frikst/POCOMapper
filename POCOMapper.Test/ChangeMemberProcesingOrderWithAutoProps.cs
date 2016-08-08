using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMapper.conventions;
using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
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

		private class MappingAuto : MappingDefinition<MappingAuto>
		{
			private MappingAuto()
			{
				ToConventions
					.SetMemberScanningPrecedence(MemberType.AutoProperty);
				Map<From, ToAuto>();
				Map<From, ToCode>();
			}
		}

		private class MappingCode : MappingDefinition<MappingCode>
		{
			private MappingCode()
			{
				ToConventions
					.SetMemberScanningPrecedence(MemberType.CodeProperty);
				Map<From, ToAuto>();
				Map<From, ToCode>();
			}
		}

		private class MappingProperty : MappingDefinition<MappingProperty>
		{
			private MappingProperty()
			{
				ToConventions
					.SetMemberScanningPrecedence(MemberType.Property);
				Map<From, ToAuto>();
				Map<From, ToCode>();
			}
		}

		[TestMethod]
		public void AutoMapping()
		{
			ToAuto retAuto = MappingAuto.Instance.Map<From, ToAuto>(new From());
			ToCode retCode = MappingAuto.Instance.Map<From, ToCode>(new From());
			Assert.AreEqual("hello world", retAuto.Data);
			Assert.IsNull(retCode.Data);
		}

		[TestMethod]
		public void CodeMapping()
		{
			ToAuto retAuto = MappingCode.Instance.Map<From, ToAuto>(new From());
			ToCode retCode = MappingCode.Instance.Map<From, ToCode>(new From());
			Assert.IsNull(retAuto.Data);
			Assert.AreEqual("hello world", retCode.Data);
		}

		[TestMethod]
		public void PropertyMapping()
		{
			ToAuto retAuto = MappingProperty.Instance.Map<From, ToAuto>(new From());
			ToCode retCode = MappingProperty.Instance.Map<From, ToCode>(new From());
			Assert.AreEqual("hello world", retAuto.Data);
			Assert.AreEqual("hello world", retCode.Data);
		}
	}
}
