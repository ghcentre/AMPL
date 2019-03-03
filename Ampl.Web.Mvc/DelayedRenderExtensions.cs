using Ampl.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Ampl.Web.Mvc
{
  /// <summary>
  /// <para>Provides a set of helper methods to render delayed content.</para>
  /// <para>You may need to delay render e.g. JavaScript code at page bottom, or, from partial views, when
  /// page sections are unavailable.</para>
  /// </summary>
  /// <example>
  /// <para>At the bottom of ~/Views/Shared/_Layout.cshtml (just before &lt;/body&gt;):</para>
  /// <code>
  /// @Html.RenderDelayed();
  /// </code>
  /// <para>Any content within <c>Html.BeginDelayed() {</c> and <c>}</c> will be rendered
  /// when <c>Html.RenderDelayed()</c> is called.</para>
  /// <para>To delay render some content use:</para>
  /// <code>
  /// @using(Html.BeginDelayed())
  /// {
  ///   &lt;script&gt;
  ///     alert('I am rendered at the end of the page!');
  ///   &lt;/script&gt;
  /// }
  /// </code>
  /// </example>
  public static class DelayedRenderExtensions
  {
    #region Get Script List / Create Script List

    private const string _delayedScriptsKey = "AMPL_delayed_scripts_";
    private static object _locker = new object();

    private static List<string> GetScriptList(HtmlHelper htmlHelper)
    {
      lock(_locker)
      {
        return (htmlHelper.ViewContext.HttpContext.Items[_delayedScriptsKey] as List<string>);
      }
    }

    private static List<string> CreateScriptList(HtmlHelper htmlHelper)
    {
      lock(_locker)
      {
        if(GetScriptList(htmlHelper) == null)
        {
          htmlHelper.ViewContext.HttpContext.Items[_delayedScriptsKey] = new List<string>();
        }
        return GetScriptList(htmlHelper);
      }
    }

    #endregion

    /// <summary>
    /// Renders delayed page content, if any.
    /// </summary>
    /// <param name="html"><see cref="HtmlHelper"/></param>
    /// <returns>A <see cref="IHtmlString"/> containing page content added by calls to BeginDelayed.</returns>
    public static IHtmlString RenderDelayed(this HtmlHelper html)
    {
      Check.NotNull(html, nameof(html));
      var list = GetScriptList(html);
      if(list == null)
      {
        return MvcHtmlString.Empty;
      }
      return new MvcHtmlString(string.Join(Environment.NewLine, list));
    }

    #region DelayedBlock

    /// <summary>
    /// Helper class for Html.BeginDelayed method.
    /// </summary>
    private class DelayedBlock : IDisposable
    {
      private HtmlHelper _htmlHelper;

      /// <summary>
      /// Initializes a new instance of the class.
      /// </summary>
      /// <param name="html"></param>
      public DelayedBlock(HtmlHelper html)
      {
        //
        // all page output is now rendered into the new TextWriter.
        //
        _htmlHelper = html;
        (_htmlHelper.ViewDataContainer as WebPageBase).OutputStack.Push(new StringWriter());
      }

      /// <summary>
      /// Releases any resources.
      /// </summary>
      public void Dispose()
      {
        //
        // render TextWriter contents and save it into delayed list.
        //
        CreateScriptList(_htmlHelper).Add((_htmlHelper.ViewDataContainer as WebPageBase).OutputStack.Pop().ToString());
      }
    }

    #endregion

    /// <summary>
    /// Delays render of all content after this method call until the <see cref="IDisposable.Dispose"/>  method is called.
    /// </summary>
    /// <param name="htmlHelper"><see cref="HtmlHelper"/></param>
    /// <returns>The method returns the object implementing the <see cref="IDisposable"/> interface. Until the object
    /// is disposed, all page output is delayed and is rendered when <see cref="RenderDelayed(HtmlHelper)"/> method is
    /// called.</returns>
    /// <example>
    /// <code>
    /// @using(Html.BeginDelayed())
    /// {
    ///   &lt;script&gt;
    ///     alert('I am rendered at the end of the page!');
    ///   &lt;/script&gt;
    /// }
    /// </code>
    /// </example>
    public static IDisposable BeginDelayed(this HtmlHelper htmlHelper)
    {
      return new DelayedBlock(htmlHelper);
    }
  }
}
