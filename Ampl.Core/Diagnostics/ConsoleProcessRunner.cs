using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Ampl.Core
{
    /// <summary>
    /// Starts a console process and captures its output and error streams.
    /// </summary>
    /// <remarks>
    /// <para>Calling <see cref="Start"/> from another thread when <see cref="IsRunning"/> is <see langword="true"/>
    /// produces unexpected results.</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// namespace CreateProcessTest
    /// {
    ///     class Program
    ///     {
    ///         static void Main(string[] args)
    ///         {
    ///             string arguments = "-extr -dir -nozip -nofix archive.zip";
    ///             string workingDir = @"C:\temp";
    ///             var runner = new ConsoleProcessRunner();
    ///             runner.StartInfo.FileName = "pkzip25.exe";
    ///             runner.StartInfo.Arguments = arguments;
    ///             runner.StartInfo.WorkingDirectory = workingDir;
    ///             runner.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866);
    ///             runner.StartInfo.StandardErrorEncoding = Encoding.GetEncoding(866);
    ///     
    ///             runner.Start();
    ///             Console.WriteLine(runner.OutputAndError);
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    public sealed class ConsoleProcessRunner
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConsoleProcessRunner()
        {
            StartInfo = new ProcessStartInfo();
        }

        /// <summary>
        /// Gets or sets the properties to pass to the Start method of the Process
        /// </summary>
        /// <value>The <see cref="ProcessStartInfo"/> object.</value>
        public ProcessStartInfo StartInfo { get; set; }

        /// <summary>
        /// Gets the Error stream contents.
        /// </summary>
        public string Error
        {
            get
            {
                lock (_lockerError)
                {
                    return _error.ToString();
                }
            }
        }
        private StringBuilder _error = new StringBuilder();
        private readonly object _lockerError = new object();

        /// <summary>
        /// Gets the Output stream contents.
        /// </summary>
        public string Output
        {
            get
            {
                lock (_lockerOutput)
                {
                    return _output.ToString();
                }
            }
        }
        private StringBuilder _output = new StringBuilder();
        private readonly object _lockerOutput = new object();

        /// <summary>
        /// Gets the contents of both Output and Error streams line-by-line
        /// in the order the process produced them.
        /// </summary>
        public string OutputAndError
        {
            get
            {
                lock (_lockerOutputAndError)
                {
                    return _outputAndError.ToString();
                }
            }
        }
        private StringBuilder _outputAndError = new StringBuilder();
        private readonly object _lockerOutputAndError = new object();

        /// <summary>
        /// Gets the process exit code.
        /// </summary>
        /// <value>The process exit code.</value>
        public int ExitCode { get; private set; }

        /// <summary>
        /// Gets the value indicating whether the process is running.
        /// </summary>
        /// <value><see langword="true"/> if the process is still running or <see langword="false"/> otherwise.</value>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Starts the process and waits for its exit.
        /// </summary>
        /// <remarks>
        /// <para>The method is <b>not</b> thread-safe.</para>
        /// <para>Calling <see cref="Start"/> from another thread when <see cref="IsRunning"/> is <see langword="true"/>
        /// produces unexpected results.</para>
        /// <para>The method sets the code page for standard output and standard error to the OEM code page
        /// (<c>CultureInfo.CurrentCulture.TextInfo.OEMCodePage</c>) which is may not available on all platforms.</para>
        /// </remarks>
        public void Start()
        {
            using var process = new Process();

            _error = new StringBuilder();
            _output = new StringBuilder();
            _outputAndError = new StringBuilder();

            process.StartInfo = StartInfo;
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage);

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.StandardErrorEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage);

            process.OutputDataReceived += new DataReceivedEventHandler(OutputDataReceived);
            process.ErrorDataReceived += new DataReceivedEventHandler(ErrorDataReceived);

            process.Start();
            IsRunning = true;

            try
            {
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();

                process.WaitForExit();
                ExitCode = process.ExitCode;
            }
            finally
            {
                IsRunning = false;
            }
        }

        void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (_lockerError)
            {
                _error.Append(e.Data + Environment.NewLine);
            }

            lock (_lockerOutputAndError)
            {
                _outputAndError.Append(e.Data + Environment.NewLine);
            }
        }

        void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (_lockerOutput)
            {
                _output.Append(e.Data + Environment.NewLine);
            }

            lock (_lockerOutputAndError)
            {
                _outputAndError.Append(e.Data + Environment.NewLine);
            }
        }
    }
}
