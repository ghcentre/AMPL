using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Ampl.Identity;

namespace Ampl.Web.Mvc
{
  public class ResourceActionAuthorizeAttribute : AuthorizeAttribute
  {

    private string _action;
    private string[] _resources;

    private const string _label = "Ampl.Web.Mvc.ResourceActionAuthorizeAttribute";

    public ResourceActionAuthorizeAttribute()
    {
    }

    public ResourceActionAuthorizeAttribute(string action, params string[] resources)
    {
      _action = action;
      _resources = resources;
    }

    public override void OnAuthorization(global::System.Web.Mvc.AuthorizationContext filterContext)
    {
      filterContext.HttpContext.Items[_label] = filterContext;
      base.OnAuthorization(filterContext);
    }

    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
      var principal = httpContext.User as ClaimsPrincipal;

      if(principal == null || principal.Identity == null)
      {
        principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.Name, "") }));
      }

      if(!string.IsNullOrWhiteSpace(_action))
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
