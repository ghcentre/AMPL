using System;
using System.Diagnostics;
using Ampl.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ampl.System.Tests
{
  [TestClass]
  public class ConsoleProcessRunnerTest
  {
    [TestMethod]
    public void Constructor_defaults()
    {
      var runner = new ConsoleProcessRunner();
      Assert.IsNotNull(runner.StartInfo);
      Assert.IsTrue(runner.ExitCode == 0);
      Assert.IsFalse(runner.IsRunning);
    }

    [TestMethod]
    public void Process_writes_to_output()
    {
      var runner = new ConsoleProcessRunner() {
        StartInfo = new ProcessStartInfo("cmd.exe", "/c dir %windir%")
      };
      runner.Start();
      Assert.IsFalse(string.IsNullOrEmpty(runner.Output));
    }

    [TestMethod]
    public void Process_writes_to_error()
    {
      var runner = new ConsoleProcessRunner() {
        StartInfo = new ProcessStartInfo("cmd.exe", "/c dir %windir%notexist")
      };
      runner.Start();
      Assert.IsFalse(string.IsNullOrEmpty(runner.Error));
    }

  }
}
