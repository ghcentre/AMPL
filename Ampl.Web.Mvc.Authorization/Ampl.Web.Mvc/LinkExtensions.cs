using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Ampl.Identity;
using Ampl.System;

namespace Ampl.Web.Mvc
{
  /// <summary>
  /// Represents support to build URLs to the resources which are authorized to the calling user.
  /// </summary>
  public static class LinkExtensions
  {
    private static string ReplaceWithActualAction(UrlHelper urlHelper, string action)
    {
      return action ?? (urlHelper?.RequestContext?.RouteData?.Values["action"] as string);
    }

    private static string ReplaceWithActualController(UrlHelper urlHelper, string controller)
    {
      return controller ?? (urlHelper.RequestContext.RouteData.Values["controller"] as string);
    }

    public static string AuthorizedAction(this UrlHelper urlHelper, string actionName)
    {
      return ClaimsAuthorization.CheckAccess(ReplaceWithActualAction(urlHelper, actionName),
                                             ReplaceWithActualController(urlHelper, null))
        ? urlHelper.Action(actionName)
        : null;
    }

    public static string AuthorizedAction(this UrlHelper urlHelper, string actionName, RouteValueDictionary routeValues)
    {
      return ClaimsAuthorization.CheckAccess(ReplaceWithActualAction(urlHelper, actionName),
                                             ReplaceWithActualController(urlHelper, null))
        ? urlHelper.Action(actionName, routeValues)
        : null;
    }

    public static string AuthorizedAction(this UrlHelper urlHelper, string actionName, object routeValues)
    {
      return ClaimsAuthorization.CheckAccess(ReplaceWithActualAction(urlHelper, actionName),
                                             ReplaceWithActualController(urlHelper, null))
        ? urlHelper.Action(actionName, routeValues)
        : null;
    }

    public static string AuthorizedAction(this UrlHelper urlHelper, string actionName, string controllerName)
    {
      return ClaimsAuthorization.CheckAccess(ReplaceWithActualAction(urlHelper, actionName),
                                             ReplaceWithActualController(urlHelper, controllerName))
        ? urlHelper.Action(actionName)
        : null;
    }

    public static string AuthorizedAction(this UrlHelper urlHelper,
                                          string actionName,
                                          string controllerName,
                                          RouteValueDictionary routeValues)
    {
      return ClaimsAuthorization.CheckAccess(ReplaceWithActualAction(urlHelper, actionName),
                                             ReplaceWithActualController(urlHelper, controllerName))
        ? urlHelper.Action(actionName, controllerName, routeValues)
        : null;
    }

    public static string AuthorizedAction(this UrlHelper urlHelper,
                                          string actionName,
                                          string controllerName,
                                          object routeValues)
    {
      return ClaimsAuthorization.CheckAccess(ReplaceWithActualAction(urlHelper, actionName),
                                             ReplaceWithActualController(urlHelper, controllerName))
        ? urlHelper.Action(actionName, controllerName, routeValues)
        : null;
    }
  }
}
