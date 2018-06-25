namespace KST.POCOMapper.TypePatterns
{
    public static class Generic<TGeneric>
    {
		internal interface IWith { }

		public abstract class With<TParam1> : IWith { }

	    public abstract class With<TParam1, TParam2> : IWith { }

	    public abstract class With<TParam1, TParam2, TParam3> : IWith { }

	    public abstract class With<TParam1, TParam2, TParam3, TParam4> : IWith { }

	    public abstract class With<TParam1, TParam2, TParam3, TParam4, TParam5> : IWith { }

	    public abstract class With<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> : IWith { }

	    public abstract class With<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> : IWith { }

	    public abstract class With<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8> : IWith { }

	    public abstract class With<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9> : IWith { }
    }
}
