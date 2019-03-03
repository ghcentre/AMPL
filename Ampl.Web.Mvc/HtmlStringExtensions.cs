using Ampl.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebPages;

namespace Ampl.Web.Mvc
{
  /// <summary>
  /// Contains the <see langword="static"/> helper methods used in Razor views.
  /// </summary>
  public static class HtmlStringExtensions
  {
    /// <summary>
    /// Renders the <paramref name="text"/> delegate only if <paramref name="htmlString"/> if not white space.
    /// </summary>
    /// <param name="htmlString">The <see cref="IHtmlString"/> to test and pass as <c>item</c> parameter.</param>
    /// <param name="text">The render delegate, usually in Razor @-form. Can be <see langword="null"/>.</param>
    /// <returns>If the <paramref name="htmlString"/> is not <see langword="null"/>, an empty string,
    /// or does not contain whitespace characters, the method returns the <see cref="HelperResult"/> object.
    /// Otherwise, the method returns <see langword="null"/>.</returns>
    /// <remarks>Use <c>@item</c> in the renderer to reference the <paramref name="htmlString"/>.</remarks>
    /// <example>
    /// <para>The following example renders a link to a private area using <c>AuthorizedActionLink</c>, and,
    /// if the user has no acces, the link is empty and the list-item element is not rendered.</para>
    /// <para>SomeView.cshtml:</para>
    /// <code>
    /// @Html.AuthorizedActionLink("Private area", "Index", "PrivateArea").IfNotEmpty(@&lt;li&gt;@item&lt;/li&gt;)
    /// </code>
    /// </example>
    public static HelperResult IfNotEmpty(this IHtmlString htmlString, Func<IHtmlString, HelperResult> text)
    {
      if(htmlString == null  || htmlString.ToHtmlString().ToNullIfWhiteSpace() == null)
      {
        return null;
      }
      return text?.Invoke(htmlString);
    }
  }
}
