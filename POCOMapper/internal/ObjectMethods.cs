using System.Reflection;

namespace POCOMapper.@internal
{
	public static class ObjectMethods
	{
		private static readonly MethodInfo aGetType = typeof(object).GetMethod("GetType", BindingFlags.Public | BindingFlags.Instance);

		public new static MethodInfo GetType()
		{
			return aGetType;
		}
	}
}
