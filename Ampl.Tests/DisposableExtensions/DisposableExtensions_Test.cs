using System;
using System.IO;
using Ampl.System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ampl.Tests.DisposableExtensions
{
  public static class DisposableExtensions
  {
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    /// <param name="obj"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    /// <example>
    /// <code>
    /// //
    /// // Reads entire text file
    /// // After the file contents are read, the StreamReader is disposed.
    /// //
    /// string contents = new StreamReader(@"c:\windows\win.ini").Use(sr => sr.ReadToEnd());
    /// </code>
    /// </example>
    public static TReturn Use<T, TReturn>(this T obj, Func<T, TReturn> func) where T : class, IDisposable
    {
      if(obj == null)
      {
        return default(TReturn);
      }

      Check.NotNull(func, nameof(func));
      using(obj)
      {
        return func(obj);
      }
    }

    //public static T Use<T>(this T obj, Action<T> action) where T : class, IDisposable
    //{
    //  if(obj == null)
    //  {
    //    return default(T);
    //  }

    //  Check.NotNull(action, nameof(action));
    //  using(obj)
    //  {
    //    action(obj);
    //  }
    //  return obj;
    //}
  }

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
