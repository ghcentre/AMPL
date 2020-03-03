using Ampl.Diagnostics;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class ConsoleProcessRunner_Tests
    {
        private class ProcessArgs
        {
            public string Program { get; set; }
            public string Args { get; set; }
        }

        private ProcessArgs CreateOsDependentProcessArgs()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new ProcessArgs() {
                    Program = "cmd.exe",
                    Args = "/c dir"
                };
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return new ProcessArgs() {
                    Program = "/bin/bash",
                    Args = "-c ls -la"
                };
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return new ProcessArgs() {
                    Program = "/bin/zsh",
                    Args = "-c ls -la"
                };
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        [Test]
        public void Ctor_SetsDefaults()
        {
            var runner = new ConsoleProcessRunner();
            Assert.IsNotNull(runner.StartInfo);
            Assert.IsTrue(runner.ExitCode == 0);
            Assert.IsFalse(runner.IsRunning);
        }

        [Test]
        public void Start_Normal_WritesToOutput()
        {
            var args = CreateOsDependentProcessArgs();
            var runner = new ConsoleProcessRunner() {
                StartInfo = new ProcessStartInfo(args.Program, args.Args)
            };

            runner.Start();

            string s = runner.Output;
            Assert.IsFalse(string.IsNullOrEmpty(s));
        }

        [Test]
        public void Start_Error_WritesToOutput()
        {
            var args = CreateOsDependentProcessArgs();
            var runner = new ConsoleProcessRunner() {
                StartInfo = new ProcessStartInfo(args.Program, args.Args + " nonexistentfile")
            };

            runner.Start();

            string s = runner.Error;
            Assert.IsFalse(string.IsNullOrEmpty(s));
        }
    }
}
