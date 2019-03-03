using Ampl.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Ampl.Web.Http
{
  public class ResourceActionAuthorizeAttribute : AuthorizeAttribute
  {
    private string _action;
    private string[] _resources;
    private readonly string _apiPrefix;

    private const string _label = "Ampl.Web.Http.ResourceActionAuthorizeAttribute";

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
    /// <remarks>Web Api prefix to separate MVC controllers authorization from Web API controllers authorization
    /// Set to <see langword="null"/> to have shared auth.</remarks>
    public ResourceActionAuthorizeAttribute(string apiPrefix)
    {
      _apiPrefix = apiPrefix;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceActionAuthorizeAttribute"/> class.
    /// </summary>
    /// <param name="action">Action to authorize against.</param>
    /// <param name="resources">Resources to authorize against.</param>
    /// <remarks>Web API prefix is set to <see langword="null"/>. </remarks>
    public ResourceActionAuthorizeAttribute(string action, params string[] resources)
    {
      _action = action;
      _resources = resources;
    }

    protected override bool IsAuthorized(HttpActionContext actionContext)
    {
      if(IsGlobalFilter)
      {
        bool skipAuthorization =
          actionContext.ActionDescriptor.GetCustomAttributes<AuthorizeAttribute>(true).Count > 0 ||
          actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AuthorizeAttribute>(true).Count > 0;
        if(skipAuthorization)
        {
          return true;
        }
      }

      var principal = (actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal);
      if(principal == null || principal.Identity == null)
      {
        principal = new ClaimsPrincipal(
          new ClaimsIdentity(
            new List<Claim>() {
              new Claim(ClaimTypes.Name, string.Empty)
            }
          )
        );
      }

      if(!string.IsNullOrWhiteSpace(_action))
      {
        return ClaimsAuthorization.CheckAccess(principal, _action, _resources);
      }
      else
      {
        return CheckAccess(actionContext, principal);
      }
    }

    protected virtual bool CheckAccess(HttpActionContext actionContext, ClaimsPrincipal principal)
    {
      string action = actionContext.ActionDescriptor.ActionName;
      string resource = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
      string prefixedResource = (_apiPrefix ?? string.Empty) + resource;

      return ClaimsAuthorization.CheckAccess(principal,
                                             action,
                                             prefixedResource);
    }

    protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
    {
      base.HandleUnauthorizedRequest(actionContext);
      bool authenticated = actionContext.RequestContext?.Principal?.Identity?.IsAuthenticated ?? false;
      actionContext.Response.StatusCode = authenticated
                                            ? global::System.Net.HttpStatusCode.Forbidden
                                            : global::System.Net.HttpStatusCode.Unauthorized;
    }
  }
}