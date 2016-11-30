using System;
using System.Web.Mvc;
using Ampl.System;

namespace Ampl.Web.Mvc.Html
{
  /// <summary>
  /// Provides <see langword="static"/> methods for ModelState validation.
  /// </summary>
  public static class HtmlHelperValidationExtensions
  {
    /// <summary>
    /// Check the Model to be non-null and the ModelState for an error.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="htmlHelper">The <see cref="HtmlHelper"/>.</param>
    /// <param name="expression">Expression that identifies the property which error state has to be checked.</param>
    /// <param name="error">The text that is returned if the ModelState for the property has an error.</param>
    /// <param name="success">The text that is returned if the ModelState for the property has no error.</param>
    /// <returns>The string specified in the <paramref name="error"/> parameter, or the string specified in the
    /// <paramref name="success"/> parameter, or <see langword="null"/> if the model is <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="htmlHelper"/> is <see langword="null"/>.</exception>
    public static MvcHtmlString ValidationErrorOrSuccess<TModel>(
      this HtmlHelper<TModel> htmlHelper,
      string expression,
      string error,
      string success)
    {
      Check.NotNull(htmlHelper, nameof(htmlHelper));

      if(htmlHelper.ViewData.Model == null)
      {
        return null;
      }

      bool hasError = HasError(
        htmlHelper,
        ModelMetadata.FromStringExpression(expression, htmlHelper.ViewData),
        expression);
      return new MvcHtmlString(hasError ? error : success);
    }

    private static bool HasError(this HtmlHelper htmlHelper, ModelMetadata modelMetadata, string expression)
    {
      string modelName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expression);
      FormContext formContext = htmlHelper.ViewContext.FormContext;
      if(formContext == null)
      {
        return false;
      }

      if(!htmlHelper.ViewData.ModelState.ContainsKey(modelName))
      {
        return false;
      }

      ModelState modelState = htmlHelper.ViewData.ModelState[modelName];
      if(modelState == null)
      {
        return false;
      }

      ModelErrorCollection modelErrors = modelState.Errors;
      if(modelErrors == null)
      {
        return false;
      }

      return (modelErrors.Count > 0);
    }
  }
}
