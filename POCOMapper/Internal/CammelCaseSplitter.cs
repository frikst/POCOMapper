using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace KST.POCOMapper.Internal
{
	internal class CammelCaseSplitter : IEnumerable<string>
	{
		private readonly string aStr;

		public CammelCaseSplitter(string str)
		{
			this.aStr = str;
		}

		#region Implementation of IEnumerable

		public IEnumerator<string> GetEnumerator()
		{
			var sb = new StringBuilder(this.aStr.Length);

			foreach (var ch in this.aStr)
			{
				if (char.IsUpper(ch))
				{
					if (sb.Length > 0)
					{
						yield return sb.ToString();
						sb.Clear();
					}

					sb.Append(char.ToLower(ch));
				}
				else
				{
					sb.Append(ch);
				}
			}

			if (sb.Length > 0)
				yield return sb.ToString();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
