using System;
using System.IO;
using Ampl.System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ampl.Tests.DisposableExtensions
{
  public class DisposableTestClass : IDisposable
  {
    public bool Disposed { get; private set; }

    public void Dispose()
    {
      Disposed = true;
    }
  }


  [TestClass]
  public class DisposableExtensions_Test
  {
    [TestMethod]
    public void Use_Null_obj_returns_null()
    {
      StreamReader arg = null;
      string result = arg.Use(x => x.ReadToEnd());
      Assert.IsNull(result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Use_Null_action_throws()
    {
      StreamReader arg = new StreamReader(@"C:\windows\win.ini");
      string result = arg.Use<StreamReader, string>(null);
    }

    [TestMethod]
    public void Use_returns()
    {
      string contents = new StreamReader(@"c:\windows\win.ini").Use(sr => sr.ReadToEnd());
      Assert.IsTrue(contents.Contains("; for 16-bit app support"));
    }

    [TestMethod]
    public void Use_Disposes()
    {
      DisposableTestClass arg = new DisposableTestClass().Use(dtc => dtc);
      Assert.IsTrue(arg.Disposed);
    }

  }
}
