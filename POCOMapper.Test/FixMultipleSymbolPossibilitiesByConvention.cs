using KST.POCOMapper.Conventions.SymbolConventions;
using KST.POCOMapper.Definition;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
    [TestFixture]
    public class FixMultipleSymbolPossibilitiesByConvention
    {
        private class From
        {
            public string X_DataData = "Boo";
            public int DataData = 5;
        }

        private class To
        {
            public int DataData;
        }

        private class Mapping : MappingSingleton<Mapping>
        {
            private Mapping()
            {
                FromConventions
                    .SetFieldConvention(new Prefix("X_", new BigCammelCase()));

                Map<From, To>();
            }
        }


        [Test]
        public void StructuringWithMultipleToPossibilitiesMappingTest()
        {
            To ret = Mapping.Instance.Map<From, To>(new From());

            Assert.AreEqual(5, ret.DataData);
        }
    }
}
