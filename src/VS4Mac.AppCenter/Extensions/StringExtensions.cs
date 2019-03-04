using System;

namespace VS4Mac.AppCenter.Extensions
{
	public static class StringExtensions
	{
		public static string UppercaseFirst(this string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			return char.ToUpper(s[0]) + s.Substring(1);
		}

		public static bool Contains(this string source, string value, StringComparison comparisonType)
		{
			return source?.IndexOf(value, comparisonType) >= 0;
		}
	}
}