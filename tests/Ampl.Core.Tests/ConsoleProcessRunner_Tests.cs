using Ampl.Diagnostics;
using NUnit.Framework;
using System.Diagnostics;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class ConsoleProcessRunner_Tests
    {
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
            var runner = new ConsoleProcessRunner() {
                StartInfo = new ProcessStartInfo("cmd.exe", "/c dir %windir%")
            };
            runner.Start();
            Assert.IsFalse(string.IsNullOrEmpty(runner.Output));
        }

        [Test]
        public void Start_Error_WritesToOutput()
        {
            var runner = new ConsoleProcessRunner() {
                StartInfo = new ProcessStartInfo("cmd.exe", "/c dir %windir%notexist")
            };
            runner.Start();
            Assert.IsFalse(string.IsNullOrEmpty(runner.Error));
        }
    }
}
