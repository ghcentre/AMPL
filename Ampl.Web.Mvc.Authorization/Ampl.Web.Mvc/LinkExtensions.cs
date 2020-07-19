using Ampl.Core;
using Ampl.Identity.Claims;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ampl.Web.Mvc
{
    /// <summary>
    /// Represents support to build URLs to the resources which are authorized to the calling user.
    /// </summary>
    public static class LinkExtensions
    {
        public static string AuthorizedAction(this UrlHelper urlHelper,
                                              string actionName)
        {
            return ActionOrNull(urlHelper,
                                actionName,
                                null,
                                () => urlHelper.Action(actionName));

        }

        public static string AuthorizedAction(this UrlHelper urlHelper,
                                              string actionName,
                                              RouteValueDictionary routeValues)
        {
            return ActionOrNull(urlHelper,
                                actionName,
                                null,
                                () => urlHelper.Action(actionName, routeValues));
        }

        public static string AuthorizedAction(this UrlHelper urlHelper,
                                              string actionName,
                                              object routeValues)
        {
            return ActionOrNull(urlHelper,
                                actionName,
                                null,
                                () => urlHelper.Action(actionName, routeValues));
        }

        public static string AuthorizedAction(this UrlHelper urlHelper,
                                              string actionName,
                                              string controllerName)
        {
            return ActionOrNull(urlHelper,
                                actionName,
                                controllerName,
                                () => urlHelper.Action(actionName, controllerName));
        }

        public static string AuthorizedAction(this UrlHelper urlHelper,
                                              string actionName,
                                              string controllerName,
                                              RouteValueDictionary routeValues)
        {
            return ActionOrNull(urlHelper,
                                actionName,
                                controllerName,
                                () => urlHelper.Action(actionName, controllerName, routeValues));
        }

        public static string AuthorizedAction(this UrlHelper urlHelper,
                                              string actionName,
                                              string controllerName,
                                              object routeValues)
        {
            return ActionOrNull(urlHelper,
                                actionName,
                                controllerName,
                                () => urlHelper.Action(actionName, controllerName, routeValues));
        }


        private static string ActionOrNull(UrlHelper urlHelper,
                                           string actionName,
                                           string controllerName,
                                           Func<string> urlHelperAction)
        {
            Check.NotNull(urlHelper, nameof(urlHelper));
            
            string actualAction = ReplaceWithActualAction(urlHelper, actionName);
            string actualController = ReplaceWithActualController(urlHelper, controllerName);

            bool accessGranted = ClaimsAuthorization.CheckAccess(actualAction, actualController);
            if (!accessGranted)
            {
                return null;
            }

            return urlHelperAction();
        }

        private static string ReplaceWithActualAction(UrlHelper urlHelper, string action)
        {
            return action ?? (urlHelper?.RequestContext?.RouteData?.Values["action"] as string);
        }

        private static string ReplaceWithActualController(UrlHelper urlHelper, string controller)
        {
            return controller ?? (urlHelper.RequestContext.RouteData.Values["controller"] as string);
        }
    }
}
