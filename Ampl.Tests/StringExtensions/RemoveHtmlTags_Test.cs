using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ampl.System;

namespace Ampl.System.Tests
{
  [TestClass]
  public class RemoveHtmlTags_Test
  {
    [TestMethod]
    public void RemoveHtmlTags_Source_Null_returns_null()
    {
      string argument = null;
      string result = argument.RemoveHtmlTags();
      Assert.IsNull(result);
    }

    [TestMethod]
    public void RemoveHtmlTags_Source_Empty_returns_empty()
    {
      string argument = string.Empty;
      string result = argument.RemoveHtmlTags();
      Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void RemoveHtmlTags_Removes()
    {
      string argument = @"
<html>
<head>
<title>Title</title>
</head>
<body>
<!-- comment -->
<ul>
  <li><a href=""one"">one text</a></li><li><a href=""two"">two text</a></li>
  <li><a href=""one"">three text</a></li><li><a href=""two"">four text</a></li>
</ul>
<script type=""text/javascript"">
  alert('hello');
</script>
</body>
</html>";
      string result = argument.RemoveHtmlTags();
      string expected = @"Title one text two text three text four text alert('hello');";
      Assert.AreEqual(expected, result);
    }
  }
}
