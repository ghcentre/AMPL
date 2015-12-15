using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace Ampl.Tests.NullCheckExtensions
{
  [TestClass]
  public class Return_Test
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
            Exception = new Exception("Outer exception", new ArgumentException("Inner exception"))
          }
        }
      };
    }


    private int? ToNullableInt(string source)
    {
      int value;
      if(int.TryParse(source, out value))
      {
        return value;
      }
      return null;
    }

    [TestMethod]
    public void Tryparse_Monad_test_null()
    {
      int somevar = 555;
      string source = "abc";
      ToNullableInt(source).Do(i => somevar = i.Value);
      Assert.AreEqual(555, somevar);
    }

    [TestMethod]
    public void Tryparse_Monad_test_notnull()
    {
      int somevar = 555;
      string source = "123";
      ToNullableInt(source).Do(i => somevar = i.Value);
      Assert.AreEqual(123, somevar);
    }

  }
}
