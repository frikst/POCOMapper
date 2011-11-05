namespace POCOMapper.mapping.@base
{
	public interface IMapping
	{
	}

	public interface IMapping<in TFrom, out TTo> : IMapping
	{
		TTo Map(TFrom from);
	}
}
