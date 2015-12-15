using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace Ampl.Tests.MonadExtensions
{
  [TestClass]
  public class With1_Test
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

    //[TestMethod]
    //public void Test1_null_1()
    //{
    //  _model = null;
    //  Error error = _model.With(m => m.Response, r => r.Error);
    //  Assert.IsNull(error);
    //}

    //[TestMethod]
    //public void Test1_null_2()
    //{
    //  _model.Response = null;
    //  Error error = _model.With(m => m.Response, r => r.Error);
    //  Assert.IsNull(error);
    //}

    //[TestMethod]
    //public void Test1_null_3()
    //{
    //  _model.Response.Error = null;
    //  Error error = _model.With(m => m.Response, r => r.Error);
    //  Assert.IsNull(error);
    //}

    //[TestMethod]
    //public void Test1_notnull()
    //{
    //  Error error = _model.With(m => m.Response, r => r.Error);
    //  Assert.IsNotNull(error);
    //}
  }
}
