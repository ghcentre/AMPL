using Ampl.Identity.Claims;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Ampl.Web.Mvc
{
    /// <summary>
    /// Performs authorization based on Action/Resource scheme.
    /// </summary>
    public class ResourceActionAuthorizeAttribute : AuthorizeAttribute
    {
        private string _action;
        private string[] _resources;

        private const string _label = "Ampl.Web.Mvc.ResourceActionAuthorizeAttribute";

        /// <summary>
        /// Gets or sets the value indicating that this attribute acts as a global filter set by GlobalFiltersCollection.
        /// </summary>
        /// <remarks>
        /// When this property is set to <see langword="true"/>, the attribute disables itself
        /// if an AuthorizeAttribute is applied to action or controller.
        /// </remarks>
        public bool IsGlobalFilter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceActionAuthorizeAttribute"/> class.
        /// </summary>
        public ResourceActionAuthorizeAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceActionAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="action">Action to authorize against.</param>
        /// <param name="resources">Resources to authorize against.</param>
        public ResourceActionAuthorizeAttribute(string action, params string[] resources)
        {
            _action = action;
            _resources = resources;
        }

        public override void OnAuthorization(global::System.Web.Mvc.AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Items[_label] = filterContext;
            if (IsGlobalFilter)
            {
                bool skipAuthorization =
                    filterContext.ActionDescriptor.IsDefined(typeof(AuthorizeAttribute), true) ||
                    filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AuthorizeAttribute), true);

                if (skipAuthorization)
                {
                    return;
                }
            }

            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!(httpContext.User is ClaimsPrincipal principal) || principal.Identity == null)
            {
                principal = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new List<Claim>() { new Claim(ClaimTypes.Name, "") }
                    )
                );
            }

            if (!string.IsNullOrWhiteSpace(_action))
            {
                return ClaimsAuthorization.CheckAccess(principal, _action, _resources);
            }
            else
            {
                var filterContext = httpContext.Items[_label] as global::System.Web.Mvc.AuthorizationContext;
                
                return CheckAccess(principal, filterContext);
            }
        }

        protected virtual bool CheckAccess(ClaimsPrincipal principal, global::System.Web.Mvc.AuthorizationContext filterContext)
        {
            var action = filterContext.RouteData.Values["action"] as string;
            var controller = filterContext.RouteData.Values["controller"] as string;

            return ClaimsAuthorization.CheckAccess(principal, action, controller);
        }

        protected override void HandleUnauthorizedRequest(global::System.Web.Mvc.AuthorizationContext filterContext)
        {
            //
            // send 401 Unauthorized to anonymous users
            // send 403 Forbidden to authenticated users
            //

            //base.HandleUnauthorizedRequest(filterContext);
            filterContext.Result = new AccessDeniedResult(filterContext.HttpContext.Request);
        }
    }
}
