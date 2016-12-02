using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace Ampl.Tests.NullCheckExtensions
{
  [TestClass]
  public class NullCheckExtensions_Test
  {
    public class ViewModel
    {
      public Response Response { get; set; }
    }

    public class Response
    {
      public Error Error { get; set; }
    }

    public class Error
    {
      public Exception Exception { get; set; }
    }


    private ViewModel _model;

    [TestInitialize]
    public void TestInitialize()
    {
      _model = new ViewModel() {
        Response = new Response() {
          Error = new Error() {
            Exception = new Exception("Outer exception", new ArgumentException())
          }
        }
      };
    }

    [TestMethod]
    public void With_Chaining()
    {
      var exception = _model.With(m => m.Response).With(r => r.Error).With(e => e.Exception);
      Assert.IsNotNull(exception);
    }

    [TestMethod]
    public void If_Chaining_false_null()
    {
      Exception innerException = _model
        .With(m => m.Response).With(r => r.Error).With(e => e.Exception)
        .If(e => e is ArgumentException)
        .With(e => e.InnerException);
      Assert.IsNull(innerException);
    }

    [TestMethod]
    public void If_Chaining_true_notnull()
    {
      Exception innerException = _model
        .With(m => m.Response).With(r => r.Error).With(e => e.Exception)
        .If(e => e.Message != null)
        .With(e => e.InnerException);
      Assert.IsNotNull(innerException);
    }

    [TestMethod]
    public void Do_Chaining()
    {
      string outerMessage = null;
      var message = _model.With(m => m.Response).With(r => r.Error).With(e => e.Exception)
        .Do(e => outerMessage = e.Message)
        .With(e => e.InnerException);
      Assert.IsTrue(outerMessage == "Outer exception");
      Assert.IsNotNull(message);
      
    }
  }
}
