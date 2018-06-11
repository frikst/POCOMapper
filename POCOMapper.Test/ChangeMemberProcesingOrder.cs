using KST.POCOMapper.Conventions;
using KST.POCOMapper.Conventions.Symbols;
using KST.POCOMapper.Definition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.POCOMapper.Test
{
	[TestClass]
	public class ChangeMemberProcesingOrder
	{
		private class From
		{
			public string aData = "field";
			public string Data { get { return "prop"; } }
			public string GetData() { return "meth"; }
		}

		private class To
		{
			public string Data;
		}

		private class MappingFields : MappingDefinition<MappingFields>
		{
			private MappingFields()
			{
				FromConventions
					.SetMemberScanningPrecedence(MemberType.Field)
					.SetFieldConvention(new Prefix("a", new BigCammelCase()));
				Map<From, To>();
			}
		}

		private class MappingProps : MappingDefinition<MappingProps>
		{
			private MappingProps()
			{
				FromConventions
					.SetMemberScanningPrecedence(MemberType.Property);
				Map<From, To>();
			}
		}

		private class MappingMethods : MappingDefinition<MappingMethods>
		{
			private MappingMethods()
			{
				FromConventions
					.SetMemberScanningPrecedence(MemberType.Method);
				Map<From, To>();
			}
		}

		[TestMethod]
		public void FieldSourceMapping()
		{
			To ret = MappingFields.Instance.Map<From, To>(new From());
			Assert.AreEqual("field", ret.Data);
		}

		[TestMethod]
		public void MethodSourceMapping()
		{
			To ret = MappingMethods.Instance.Map<From, To>(new From());
			Assert.AreEqual("meth", ret.Data);
		}

		[TestMethod]
		public void PropertySourceMapping()
		{
			To ret = MappingProps.Instance.Map<From, To>(new From());
			Assert.AreEqual("prop", ret.Data);
		}

		[TestMethod]
		public void FieldSourceSynchronization()
		{
			To ret = new To();
			MappingFields.Instance.Synchronize(new From(), ref ret);
			Assert.AreEqual("field", ret.Data);
		}

		[TestMethod]
		public void MethodSourceSynchronization()
		{
			To ret = new To();
			MappingMethods.Instance.Synchronize(new From(), ref ret);
			Assert.AreEqual("meth", ret.Data);
		}

		[TestMethod]
		public void PropertySourceSynchronization()
		{
			To ret = new To();
			MappingProps.Instance.Synchronize(new From(), ref ret);
			Assert.AreEqual("prop", ret.Data);
		}
	}
}
