using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Ampl.Web.Mvc.Html
{
  /// <summary>
  /// 
  /// </summary>
  public static class ShortNameExtensions
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="html"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static MvcHtmlString ShortName(this HtmlHelper html, string expression)
    {
      return ShortNameHelper(
        ModelMetadata.FromStringExpression(expression, html.ViewData),
        expression);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="html"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static MvcHtmlString ShortNameFor<TModel, TValue>(
      this HtmlHelper<IEnumerable<TModel>> html,
      Expression<Func<TModel, TValue>> expression)
    {
      return ShortNameHelper(
        ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>()),
        ExpressionHelper.GetExpressionText(expression));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="html"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static MvcHtmlString ShortNameFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression)
    {
      return ShortNameHelper(
        ModelMetadata.FromLambdaExpression(expression, html.ViewData),
        ExpressionHelper.GetExpressionText(expression));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static MvcHtmlString ShortNameForModel(this HtmlHelper html)
    {
      return ShortNameHelper(html.ViewData.ModelMetadata, String.Empty);
    }

    internal static MvcHtmlString ShortNameHelper(ModelMetadata metadata, string htmlFieldName)
    {
      // We don't call ModelMetadata.GetDisplayName here because we want to fall back to the field name rather than the ModelType.
      // This is similar to how the LabelHelpers get the text of a label.
      string resolvedDisplayName =
        metadata.ShortDisplayName ??
        metadata.DisplayName ??
        metadata.PropertyName ??
        htmlFieldName.Split('.').Last();

      return new MvcHtmlString(HttpUtility.HtmlEncode(resolvedDisplayName));
    }
  }
}
