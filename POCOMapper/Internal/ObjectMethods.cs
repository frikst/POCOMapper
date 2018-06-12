using System.Reflection;

namespace KST.POCOMapper.Internal
{
	internal static class ObjectMethods
	{
		private static readonly MethodInfo aGetType = typeof(object).GetMethod(nameof(object.GetType), BindingFlags.Public | BindingFlags.Instance);

		public new static MethodInfo GetType()
		{
			return aGetType;
		}
	}
}
