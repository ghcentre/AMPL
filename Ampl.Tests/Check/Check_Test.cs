using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;
using System.Threading;

namespace Ampl.System.Tests
{
  [TestClass]
  public class Check_Test
  {
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Check_NotNull_null_arg_throws()
    {
      string arg = null;
      try
      {
        string arg2 = Check.NotNull(arg);
      }
        catch(ArgumentNullException)
      {
        throw;
      }

    }

    [TestMethod]
    public void Check_NotNull_notnull_arg_returns_same_reference()
    {
      var arg1 = new Uri("http://ghcentre.com");
      var arg2 = Check.NotNull(arg1, "arg1");
      Assert.AreEqual(arg1, arg2);
    }

    [TestMethod]
    public void Check_NotNull_null_arg_null_paramname()
    {
      Thread.CurrentThread.CurrentCulture = new global::System.Globalization.CultureInfo("en-US");
      Thread.CurrentThread.CurrentUICulture = new global::System.Globalization.CultureInfo("en-US");

      string arg = null;
      try
      {
        string val = Check.NotNull(arg, null);
      }
      catch(Exception e)
      {
        Assert.IsFalse(e.Message.ToLowerInvariant().Contains("parameter"));
      }
    }

    [TestMethod]
    public void Check_NotNull_null_arg_notnull_paramname()
    {
      Thread.CurrentThread.CurrentCulture = new global::System.Globalization.CultureInfo("en-US");
      Thread.CurrentThread.CurrentUICulture = new global::System.Globalization.CultureInfo("en-US");

      string arg = null;
      try
      {
        string val = Check.NotNull(arg, "arg");
      }
      catch(Exception e)
      {
        Assert.IsTrue(e.Message.ToLowerInvariant().Contains("parameter"));
      }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Check_NotEmptyString_null_arg_throws_arumentnull()
    {
      string arg = null;
      string val = Check.NotNullOrEmptyString(arg);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = false)]
    public void Check_NotEmptyString_empty_arg_throws_argument()
    {
      string arg = "";
      string val = Check.NotNullOrEmptyString(arg);
    }

    [TestMethod]
    public void Check_NotEmptyString_empty_arg_notnull_paramname()
    {
      Thread.CurrentThread.CurrentCulture = new global::System.Globalization.CultureInfo("en-US");
      Thread.CurrentThread.CurrentUICulture = new global::System.Globalization.CultureInfo("en-US");

      string arg = "";
      try
      {
        string val = Check.NotNullOrEmptyString(arg, "arg");
      }
      catch(Exception e)
      {
        Assert.IsTrue(e.Message.ToLowerInvariant().Contains("parameter"));
      }
    }

    [TestMethod]
    public void Check_NotEmptyString_empty_arg_null_paramname()
    {
      Thread.CurrentThread.CurrentCulture = new global::System.Globalization.CultureInfo("en-US");
      Thread.CurrentThread.CurrentUICulture = new global::System.Globalization.CultureInfo("en-US");

      string arg = "";
      try
      {
        string val = Check.NotNullOrEmptyString(arg, null);
      }
      catch(Exception e)
      {
        Assert.IsFalse(e.Message.ToLowerInvariant().Contains("parameter"));
      }
    }

    [TestMethod]
    public void Check_NotEmptyString_notempty_arg_returns_same_reference()
    {
      string arg = "This is a test string";
      string val = Check.NotNullOrEmptyString(arg, "arg");
      Assert.ReferenceEquals(arg, val);
    }

    [TestMethod]
    public void Check_NotEmptyString_empty_localization()
    {
      Thread.CurrentThread.CurrentCulture = new global::System.Globalization.CultureInfo("en-US");
      Thread.CurrentThread.CurrentUICulture = new global::System.Globalization.CultureInfo("en-US");

      string arg = "";
      try
      {
        string val = Check.NotNullOrEmptyString(arg, null);
      }
      catch(Exception e)
      {
        Assert.IsTrue(e.Message.ToLowerInvariant().Contains("value"));
      }

      Thread.CurrentThread.CurrentCulture = new global::System.Globalization.CultureInfo("ru-RU");
      Thread.CurrentThread.CurrentUICulture = new global::System.Globalization.CultureInfo("ru-RU");

      try
      {
        string val = Check.NotNullOrEmptyString(arg, null);
      }
      catch(Exception e)
      {
        Assert.IsTrue(e.Message.ToLowerInvariant().Contains("значение"));
      }


    }


  }
}
