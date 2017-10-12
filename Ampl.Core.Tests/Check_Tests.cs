using Ampl.System;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Core.Tests
{
  [TestFixture]
  public class Check_Tests
  {
    [Test]
    public void Check_NotNull_NullArg_Throws()
    {
      // arrange
      string arg = null;
      // act-assert
      Assert.Throws<ArgumentNullException>(() => Check.NotNull(arg));
    }

    [Test]
    public void Check_NotNull_NotNullArg_ReturnsSameReference()
    {
      string arg = "Not null string";
      string result = Check.NotNull(arg);
      Assert.That(result, Is.SameAs(arg));
    }

    [Test]
    public void Check_NotNull_NullArgWithParamName_ThrowsWithParamNameInMessage()
    {
      // arrange
      string arg = null;
      try
      {
        // act
        Check.NotNull(arg, "ThisIsTheArg");
      }
      catch(ArgumentNullException e)
      {
        // assert
        Assert.That(e.Message, Does.Contain("ThisIsTheArg"));
      }
    }

    [Test]
    public void Check_NotNullOrEmptyString_NullArg_ThrowsArgumentNullException()
    {
      // arrange
      string arg = null;
      // act-assert
      Assert.Throws<ArgumentNullException>(() => Check.NotNullOrEmptyString(arg));
    }

    [Test]
    public void Check_NotNullOrEmptyString_EmptyArg_ThrowsArgumentException()
    {
      // arrange
      string arg = string.Empty;
      // act-assert
      Assert.Throws<ArgumentException>(() => Check.NotNullOrEmptyString(arg));
    }

    [Test]
    public void Check_NotNullOrEmptyString_NullArgWithParamName_ThrowsWithParamNameInMessage()
    {
      // arrange
      string arg = null;
      try
      {
        // act
        Check.NotNullOrEmptyString(arg, "ThisIsArgumentName");
      }
      catch(Exception e)
      {
        // assert
        Assert.That(e.Message, Does.Contain("ThisIsArgumentName"));
      }
    }

    [Test]
    public void Check_NotNullOrEmptyString_NotNullArg_ReturnsSameReference()
    {
      string arg = "This is some argument";
      string result = Check.NotNullOrEmptyString(arg, "ThisIsArgument");
      Assert.That(result, Is.SameAs(arg));
    }
  }
}
