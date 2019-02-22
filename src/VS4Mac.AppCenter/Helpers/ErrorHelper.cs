using System;
using VS4Mac.AppCenter.Exceptions;

namespace VS4Mac.AppCenter.Helpers
{
	public static class ErrorHelper
	{
		public static void CheckError(string content)
		{
			if (content.StartsWith("Error", StringComparison.InvariantCultureIgnoreCase))
				throw new AppCenterException(content);
		}
	}
}