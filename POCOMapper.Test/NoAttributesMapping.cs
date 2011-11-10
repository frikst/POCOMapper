using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using POCOMapper.definition;

namespace POCOMapper.Test
{
	[TestClass]
	public class NoAttributesMapping
	{
		private class From
		{

		}

		private class To
		{

		}

		private class Mapping : MappingDefinition
		{
			private Mapping()
			{
				CreateMap<From, To>();
			}

			public static MappingImplementation Instance
			{
				get { return GetInstance<Mapping>(); }
			}
		}

		[TestMethod]
		public void CheckCorrectTypeTest()
		{
			To ret = Mapping.Instance.Map<From, To>(new From());
		}
	}
}
