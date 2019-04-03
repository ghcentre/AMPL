using Ampl.Core;
using Ampl.Identity.Claims;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Ampl.Web.Mvc.Html
{
    /// <summary>
    /// Represents support for HTML links to the resources which are authorized to the calling user.
    /// </summary>
    public static class LinkExtensions
    {
        /// <summary>
        /// Returns an anchor element (a element) for the specified link text, action, and controller.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="linkText">Link text.</param>
        /// <param name="actionName">Action name.</param>
        /// <param name="controllerName">Controller name</param>
        /// <returns>An anchor element (a element).</returns>
        /// <remarks>If the calling user is not authorized to call the resource identified by the
        /// <paramref name="actionName"/> and <paramref name="controllerName"/>, the method returns
        /// <see langword="null" />.</remarks>
        public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper,
                                                         string linkText,
                                                         string actionName,
                                                         string controllerName)
        {
            Check.NotNull(htmlHelper, nameof(htmlHelper));
            string actualControllerName = controllerName
                                          ?? htmlHelper.ViewContext?.RouteData?.Values["controller"]?.ToString();

            if(ClaimsAuthorization.CheckAccess(actionName, actualControllerName))
            {
                return htmlHelper.ActionLink(linkText, actionName, actualControllerName);
            }
            return null;
        }

        /// <summary>
        /// Returns an anchor element (a element) for the specified link text, action, controller, route values
        /// and HTML attributes.
        /// </summary>        
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="linkText">Link text.</param>
        /// <param name="actionName">Action name.</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeVales">Route values.</param>
        /// <param name="htmlAttributes">Anchor element HTML attributes.</param>
        /// <returns>An anchor element (a element).</returns>
        /// <remarks>If the calling user is not authorized to call the resource identified by the
        /// <paramref name="actionName"/> and <paramref name="controllerName"/>, the method returns
        /// <see langword="null" />.</remarks>
        public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper,
                                                         string linkText,
                                                         string actionName,
                                                         string controllerName,
                                                         object routeVales,
                                                         object htmlAttributes)
        {
            Check.NotNull(htmlHelper, nameof(htmlHelper));
            string actualControllerName = controllerName
                                          ?? htmlHelper.ViewContext?.RouteData?.Values["controller"]?.ToString();

            if(ClaimsAuthorization.CheckAccess(actionName, actualControllerName))
            {
                return htmlHelper.ActionLink(linkText, actionName, actualControllerName, routeVales, htmlAttributes);
            }
            return null;
        }

        /// <summary>
        /// Returns an anchor element (a element) for the specified link text, action, route values
        /// and HTML attributes. The controller is the same as the calling controller.
        /// </summary>        
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="linkText">Link text.</param>
        /// <param name="actionName">Action name.</param>
        /// <param name="routeValues">Route values.</param>
        /// <param name="htmlAttributes">Anchor element HTML attributes.</param>
        /// <returns>An anchor element (a element).</returns>
        /// <remarks>If the calling user is not authorized to call the resource
        /// identified by the <paramref name="actionName"/> and <paramref name="routeValues"/>,
        /// the method returns <see langword="null" />.</remarks>
        public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper,
                                                         string linkText,
                                                         string actionName,
                                                         object routeValues,
                                                         object htmlAttributes)
        {
            return AuthorizedActionLink(htmlHelper,
                                        linkText,
                                        actionName,
                                        null /*controllerName */,
                                        routeValues,
                                        htmlAttributes);
        }
    }
}
