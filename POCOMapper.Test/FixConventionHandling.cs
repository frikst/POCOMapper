using System;
using System.Collections.Generic;
using System.Text;
using KST.POCOMapper.Conventions;
using KST.POCOMapper.Conventions.SymbolConventions;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Mapping.Object;
using KST.POCOMapper.Validation;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
    [TestFixture]
    class FixConventionHandling
    {
        private class From
        {
            private int C_X_ID { get; set; }
        }

        private class To
        {
            [ShouldBeMapped]
            private int X { get; set; }
        }

        private class Mapping : MappingSingleton<Mapping>
        {
            private Mapping()
            {

                this.FromConventions
                    .SetFieldConvention(new Suffix(new Suffix(new Suffix(new Prefix("V_", new Prefix("A_", new Prefix("C_", new Prefix("S_", new BigCammelCase())))), "_C"), "_ID"), "_d"))
                    .SetPropertyConvention(new Suffix(new Suffix(new Prefix("V_", new Prefix("A_", new Prefix("C_", new Prefix("S_", new BigCammelCase())))), "_C"), "_ID"))
                    .SetMemberScanningPrecedence(MemberType.Field, MemberType.Property);

                this.ToConventions
                    .SetMemberScanningPrecedence(MemberType.AutoProperty);

                this.Map<From, To>()
                    .ObjectMappingRules();
            }
        }

        [Test]
        public void ValidateTest()
        {
            Mapping.Instance.Mappings.AcceptForAll(new MappingValidationVisitor());
        }
    }
}
