using System;

namespace KST.POCOMapper.Exceptions
{
    public class InvalidPlaceholderTypeException : Exception
    {
	    public InvalidPlaceholderTypeException(Type placeholderType)
			: base($"{placeholderType.Name} is not valid placeholder type")
	    {
	    }
    }
}
