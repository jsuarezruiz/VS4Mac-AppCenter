using System.Diagnostics;
using System.Threading;
using VS4Mac.AppCenter.Models;

namespace VS4Mac.AppCenter.Helpers
{
	public static class BashHelper
	{
		public static BashResult ExecuteBashCommand(string command)
		{
			command = command.Replace("\"", "\"\"");

			var proc = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "/bin/bash",
					Arguments = "-c \"" + command + "\"",
					UseShellExecute = false,
					CreateNoWindow = true,
					RedirectStandardError = true,
					RedirectStandardOutput = true
				}
			};

			proc.Start();
			proc.WaitForExit();

			return new BashResult
			{
				Code = proc.ExitCode,
				Output = proc.StandardOutput.ReadToEnd()
			};
		}

		public static BashResult ExecuteBashCommandWithConfirmation(string command)
		{
			command = command.Replace("\"", "\"\"");

			var proc = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "/bin/bash",
					Arguments = "-c \"" + command + "\"",
					CreateNoWindow = true,
					UseShellExecute = false,
					RedirectStandardError = true,
					RedirectStandardOutput = true,
					RedirectStandardInput = true
				}
			};

			proc.Start();

			Thread.Sleep(1000);

			// Write a "y" to the process's input
			proc.StandardInput.Write("y");
			proc.StandardInput.Flush();
			proc.StandardInput.Close();

			Thread.Sleep(1000);

			proc.WaitForExit();

			return new BashResult
			{
				Code = proc.ExitCode,
				Output = proc.StandardOutput.ReadToEnd()
			};
		}
	}
}