using System;
using System.Collections.Generic;
using System.Text;
using KST.POCOMapper.Definition;
using KST.POCOMapper.Exceptions;
using KST.POCOMapper.Mapping.Standard;
using NUnit.Framework;

namespace KST.POCOMapper.Test
{
	[TestFixture]
	public class EnumCast
    {
	    private enum Enum1
	    {
			A, B, C
	    }

	    private enum Enum2
	    {
		    A, B, C
	    }

	    private class Mapping : MappingDefinition<Mapping>
	    {
		    private Mapping()
		    {
			    Map<Enum1, int>()
				    .CastRules();

			    Map<Enum1, Enum2>()
				    .CastRules();

			    Map<Enum2, bool>()
				    .CastRules();

		    }
	    }

	    [Test]
	    public void Enum1ToInt()
	    {
		    var ret = Mapping.Instance.Map<Enum1, int>(Enum1.B);
			Assert.AreEqual(1, ret);
	    }

	    [Test]
	    public void Enum1ToEnum2()
	    {
		    var ret = Mapping.Instance.Map<Enum1, Enum2>(Enum1.B);
		    Assert.AreEqual(Enum2.B, ret);
	    }

	    [Test]
	    public void Enum2ToIntInvalid()
	    {
		    Assert.Throws<UnknownMappingException>(() => Mapping.Instance.Map<Enum2, int>(Enum2.B));
	    }

	    [Test]
	    public void Enum2ToBoolInvalid()
	    {
		    Assert.Throws<InvalidMappingException>(() => Mapping.Instance.Map<Enum2, bool>(Enum2.B));
	    }
    }
}
