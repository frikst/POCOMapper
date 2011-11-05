namespace POCOMapper.conventions
{
	public class Conventions
	{
		public Conventions()
		{
			this.Attributes = new BigCammelCase();
			this.Methods = new BigCammelCase();
			this.Properties = new BigCammelCase();
		}

		public ISymbolParser Attributes { get; private set; }
		public ISymbolParser Methods { get; private set; }
		public ISymbolParser Properties { get; private set; }

		public Conventions SetAttributeConvention(ISymbolParser parser)
		{
			this.Attributes = parser;
			return this;
		}

		public Conventions SetMethodConvention(ISymbolParser parser)
		{
			this.Methods = parser;
			return this;
		}

		public Conventions SetPropertyConvention(ISymbolParser parser)
		{
			this.Properties = parser;
			return this;
		}
	}
}
