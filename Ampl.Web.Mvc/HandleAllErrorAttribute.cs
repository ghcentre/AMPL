using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Ampl.System;

namespace Ampl.Web.Mvc
{
  /// <summary>
  /// <para>Represents an attribute that is used to handle an exception that is thrown by an action method.</para>
  /// <para>Unlike the <see cref="HandleErrorAttribute"/>, this attribute handles all types of exceptions.</para>
  /// </summary>
  /// <example>
  /// <para><b>Usage:</b></para>
  /// <para>Change the <b>~/App_Code/FilterConfig.cs</b> as following:</para>
  /// <code>
  /// public static void RegisterGlobalFilters(GlobalFilterCollection filters)
  /// {
  ///   filters.Add(new HandleAllErrorAttribute());
  /// }
  /// </code>
  /// <para>In <b>web.config</b>, add the <b>customErrors</b> element to the <b>system.web</b> element:</para>
  /// <code>
  /// &lt;configuration&gt;
  ///   &lt;system.web&gt;
  ///     &lt;customErrors mode="On" /&gt;
  ///   &lt;/system.web&gt;
  /// &lt;/configuration&gt;
  /// </code>
  /// </example>
  public class HandleAllErrorAttribute : HandleErrorAttribute
  {
    /// <summary>
    /// Called when an exception occurs.
    /// </summary>
    /// <param name="filterContext">The action-filter context.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="filterContext"/> is null.</exception>
    public override void OnException(ExceptionContext filterContext)
    {
      Check.NotNull(filterContext, nameof(filterContext));

      //
      // If custom errors are disabled, we need to let the normal ASP.NET exception handler
      // execute so that the user can see useful debugging information.
      //
      if(filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
      {
        return;
      }

      Exception exception = filterContext.Exception;

      if(!ExceptionType.IsInstanceOfType(exception))
      {
        return;
      }

      string controllerName = (string)filterContext.RouteData.Values["controller"];
      string actionName = (string)filterContext.RouteData.Values["action"];
      HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception,
        controllerName, actionName);

      filterContext.Result = new ViewResult() {
        ViewName = View,
        MasterName = Master,
        ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
        TempData = filterContext.Controller.TempData
      };
      filterContext.ExceptionHandled = true;
      filterContext.HttpContext.Response.Clear();
      filterContext.HttpContext.Response.StatusCode = new HttpException(null, exception).GetHttpCode();

      //
      // Certain versions of IIS will sometimes use their own error page when
      // they detect a server error. Setting this property indicates that we
      // want it to try to render ASP.NET MVC's error page instead.
      //
      filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
    }
  }
}
