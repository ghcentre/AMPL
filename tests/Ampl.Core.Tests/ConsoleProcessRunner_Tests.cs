using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class ConsoleProcessRunner_Tests
    {
        #region Utility classes and methods

        private record ProcessArgs(string Program, string Args);

        private static ProcessArgs CreateOsDependentProcessArgs()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new ProcessArgs("cmd.exe", "/c dir");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return new ProcessArgs("/bin/bash", "-c ls -la");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return new ProcessArgs("/bin/zsh", "-c ls -la");
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        private ProcessArgs _processArgs = new("invalid", "invalid");

        #endregion

        #region Setup

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [SetUp]
        public void Setup()
        {
            _processArgs = CreateOsDependentProcessArgs();
        }

        [TearDown]
        public void Teardown()
        {
            _processArgs = default!;
        }

        #endregion

        #region Ctor

        [Test]
        public void Ctor_SetsDefaults()
        {
            var runner = new ConsoleProcessRunner();
            Assert.IsNotNull(runner.StartInfo);
            Assert.IsTrue(runner.ExitCode == 0);
            Assert.IsFalse(runner.IsRunning);
        }

        #endregion

        #region Start

        [Test]
        public void Start_Normal_WritesToOutput()
        {
            var runner = new ConsoleProcessRunner()
            {
                StartInfo = new ProcessStartInfo(_processArgs.Program, _processArgs.Args)
            };

            runner.Start();

            string s = runner.Output;
            Assert.IsFalse(string.IsNullOrEmpty(s));
        }

        [Test]
        public void Start_Error_WritesToOutput()
        {
            var runner = new ConsoleProcessRunner()
            {
                StartInfo = new ProcessStartInfo(_processArgs.Program, _processArgs.Args + " nonexistentfile")
            };

            runner.Start();

            string s = runner.Error;
            Assert.IsFalse(string.IsNullOrEmpty(s));
        }

        #endregion
    }
}
