/* Description:
This class is a static utility class which provides a single method Run(string commandLine) that runs a command using the bash shell.
It creates a process with the specified command line and redirects the standard output and error to be read by the process.
It also starts listening for the output and error data. If the process doesn't exit within 500ms, it throws a timeout exception. 
If the process exits with a non-zero code it throws an exception with the output and error data of the process.
*/
//TODO: Find where is this called and why
#region

using System;
using System.Diagnostics;
using System.Text;

#endregion

internal static class BashRunner
{
    /// <summary>
    ///     Runs a command using the bash shell
    /// </summary>
    /// <param name="commandLine"> command to be executed </param>
    /// <returns> output of the command </returns>
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static string Run(string commandLine)
    {
        // Initialize string builders to store output and error messages
        var errorBuilder = new StringBuilder();
        var outputBuilder = new StringBuilder();
        // Creates the argument string for the bash process
        var arguments = $"-c \"{commandLine}\"";
        // Initialize the process with the required settings
        using (var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = false
            }
        })
        {
            // Start the process
            process.Start();
            // register event handlers to handle the output and error data
            process.OutputDataReceived += (sender, args) => { outputBuilder.AppendLine(args.Data); };
            process.BeginOutputReadLine();
            process.ErrorDataReceived += (sender, args) => { errorBuilder.AppendLine(args.Data); };
            process.BeginErrorReadLine();
            // Wait for the process to exit with a timeout of 500ms
            if (!process.WaitForExit(500))
            {
                var timeoutError = $@"Process timed out. Command line: bash {arguments}.
Output: {outputBuilder}
Error: {errorBuilder}";
                throw new Exception(timeoutError);
            }
            // If the process exits with a non-zero code, throw an exception
            if (process.ExitCode == 0)
            {
                return outputBuilder.ToString();
            }

            var error = $@"Could not execute process. Command line: bash {arguments}.
Output: {outputBuilder}
Error: {errorBuilder}";
            throw new Exception(error);
        }
    }
}