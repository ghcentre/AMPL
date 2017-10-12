using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ampl.System;

namespace Ampl.Core.Tests
{
  [TestFixture]
  public class DictionaryExtensions_Tests
  {
    [Test]
    public void IncludeObjectProperties_NullThis_Throws()
    {
      // arrange
      Dictionary<string, object> dic = null;
      var anon = new { IntProp = 5, StringProp = "String" };
      // act-assert
      Assert.Throws<ArgumentNullException>(() => dic.IncludeObjectProperties(anon));
    }

    [Test]
    public void IncludeObjectPropertis_NullArg_Throws()
    {
      // arrange
      var dic = new Dictionary<string, object>();
      object anon = null;
      // act-assert
      Assert.Throws<ArgumentNullException>(() => dic.IncludeObjectProperties(anon));
    }

    [Test]
    public void IncludeObjectProperties_Add_AddsAllProperties()
    {
      // arrange
      var dic = new Dictionary<string, object>();
      var anon = new { IntProp = 5, StringProp = "String" };
      // act
      dic.IncludeObjectProperties(anon);
      // assert
      Assert.That(dic.Count, Is.EqualTo(2));
    }

    [Test]
    public void IncludeObjectProperties_Add_ReferencesSameObjects()
    {
      // arrange
      var dic = new Dictionary<string, object>();
      var anon = new { IntProp = 5, StringProp = "String" };
      // act
      dic.IncludeObjectProperties(anon);
      // assert
      Assert.That((int)dic["IntProp"], Is.EqualTo(5));
      Assert.That((string)dic["StringProp"], Is.SameAs(anon.StringProp));
    }

    [Test]
    public void IncludeObjectProperties_AddToNonEmpty_Replaces()
    {
      // arrange
      var dic = new Dictionary<string, object>() { { "StringProp", "OldString" } };
      var anon = new { IntProp = 5, StringProp = "String" };
      // act
      dic.IncludeObjectProperties(anon);
      // assert
      Assert.That(dic.Count, Is.EqualTo(2));
      Assert.That((string)dic["StringProp"], Is.SameAs(anon.StringProp));
      Assert.That((string)dic["StringProp"], Is.Not.EqualTo("OldString"));
    }
  }
}
