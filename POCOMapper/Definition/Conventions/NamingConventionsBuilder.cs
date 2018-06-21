using System.Collections.Generic;
using System.Linq;
using KST.POCOMapper.Conventions;
using KST.POCOMapper.Conventions.SymbolConventions;
using KST.POCOMapper.Exceptions;

namespace KST.POCOMapper.Definition.Conventions
{
    public abstract class NamingConventionsBuilder
    {
	    internal NamingConventionsBuilder(NamingConventions.Direction direction)
	    {
		    this.ConventionDirection = direction;

		    this.Fields = new BigCammelCase();
		    this.Methods = new BigCammelCase();
		    this.Properties = new BigCammelCase();

		    this.MemberScanningPrecedence = new[] { MemberType.Property, MemberType.Method, MemberType.Field };
	    }

	    protected NamingConventions.Direction ConventionDirection { get; }

	    protected ISymbolConvention Fields { get; private set; }
	    protected ISymbolConvention Methods { get; private set; }
	    protected ISymbolConvention Properties { get; private set; }

	    protected IEnumerable<MemberType> MemberScanningPrecedence { get; private set; }

	    public NamingConventionsBuilder SetFieldConvention(ISymbolConvention parser)
	    {
		    this.Fields = parser;
		    return this;
	    }

	    public NamingConventionsBuilder SetMethodConvention(ISymbolConvention parser)
	    {
		    this.Methods = parser;
		    return this;
	    }

	    public NamingConventionsBuilder SetPropertyConvention(ISymbolConvention parser)
	    {
		    this.Properties = parser;
		    return this;
	    }

	    public NamingConventionsBuilder SetMemberScanningPrecedence(params MemberType[] precedence)
	    {
		    if (precedence.GroupBy(x => x).Any(x => x.Count() > 1))
			    throw new InvalidConventionException("Parameters of the SetMemberScanningPrecedence method must be uniqe");

		    if (precedence.Any(x => x == MemberType.Property) && precedence.Any(x => x == MemberType.AutoProperty || x == MemberType.CodeProperty))
			    throw new InvalidConventionException("Parameters of the SetMemberScanningPrecedence method must be uniqe");

		    this.MemberScanningPrecedence = precedence;
		    return this;
	    }

	    internal abstract NamingConventions Finish();
    }
}
