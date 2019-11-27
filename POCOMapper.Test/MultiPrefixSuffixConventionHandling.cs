using KST.POCOMapper.Conventions.SymbolConventions;
using KST.POCOMapper.Members;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
    [TestFixture]
    class MultiPrefixSuffixConventionHandling
    {
        [Test]
        public void ValidateTest()
        {
            var parser = new Suffix(new Prefix(new[] {"V_", "A_", "C_", "S_"}, new BigCammelCase()), new[] {"_C", "_ID", "_d"});

            Assert.AreEqual(new Symbol(new[] {"x"}), parser.Parse("C_X_ID"));
        }
    }
}
