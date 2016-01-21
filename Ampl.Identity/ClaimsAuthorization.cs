﻿using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Ampl.System;

namespace Ampl.Identity
{
  /// <summary>
  /// Provides direct access methods for evaluating authorization policy.
  /// </summary>
  public static class ClaimsAuthorization
  {
    /// <summary>
    /// Default action claim type.
    /// </summary>
    public const string ActionType = "http://application/claims/authorization/action";

    /// <summary>
    /// Default resource claim type.
    /// </summary>
    public const string ResourceType = "http://application/claims/authorization/resource";

    /// <summary>
    /// Gets or sets the <see cref="bool"/> value indicating that the <see cref="AuthorizationManager"/> must
    /// use own implementation.
    /// </summary>
    /// <value>A <see cref="bool"/> value indicating use of own implementation.</value>
    public static bool EnforceAuthorizationManagerImplementation { get; set; }

    public static ClaimsAuthorizationManager CustomAuthorizationManager { get; set; }

    private static Lazy<ClaimsAuthorizationManager> _claimsAuthorizationManager = new Lazy<ClaimsAuthorizationManager>(
      () => new IdentityConfiguration().ClaimsAuthorizationManager);

    /// <summary>
    /// Gets the registered authorization manager.
    /// </summary>
    public static ClaimsAuthorizationManager AuthorizationManager
    {
      get
      {
        if(CustomAuthorizationManager != null)
        {
          return CustomAuthorizationManager;
        }

        return _claimsAuthorizationManager.Value;
      }
    }

    static ClaimsAuthorization()
    {
      EnforceAuthorizationManagerImplementation = true;
    }

    /// <summary>
    /// Checks the authorization policy.
    /// </summary>
    /// <param name="resource">The resource.</param>
    /// <param name="action">The action.</param>
    /// <returns>true when authorized, otherwise false</returns>
    public static bool CheckAccess(string action, params string[] resources)
    {
      //Check.NotNull(action, "action");
      return CheckAccess(ClaimsPrincipal.Current, action, resources);
    }

    public static bool CheckAccess(ClaimsPrincipal principal, string action, params string[] resources)
    {
      var context = CreateAuthorizationContext(
          principal,
          action,
          resources);

      return CheckAccess(context);
    }

    public static bool CheckAccess(IPrincipal principal, string action, params string[] resources)
    {
      var claimsPrincipal = principal as ClaimsPrincipal;
      if(claimsPrincipal == null)
      {
        throw new InvalidOperationException("Principal is not a ClaimsPrincipal");
      }

      return CheckAccess(claimsPrincipal, action, resources);
    }


    /// <summary>
    /// Checks the authorization policy.
    /// </summary>
    /// <param name="actions">The actions.</param>
    /// <param name="resources">The resources.</param>
    /// <returns>true when authorized, otherwise false</returns>
    public static bool CheckAccess(Collection<Claim> actions, Collection<Claim> resources)
    {
      Check.NotNull(actions, "actions");
      Check.NotNull(resources, "resources");

      return CheckAccess(new AuthorizationContext(ClaimsPrincipal.Current, resources, actions));
    }

    /// <summary>
    /// Checks the authorization policy.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="resources">The resources.</param>
    /// <returns>true when authorized, otherwise false</returns>
    public static bool CheckAccess(string action, params Claim[] resources)
    {
      Check.NotNull(action, "action");
      Check.NotNull(resources, "resources");

      var actionCollection = new Collection<Claim>();
      actionCollection.Add(new Claim(ActionType, action));
      var resourceCollection = new Collection<Claim>();
      foreach(var resource in resources)
      {
        resourceCollection.Add(resource);
      }

      return CheckAccess(new AuthorizationContext(
          ClaimsPrincipal.Current, resourceCollection, actionCollection));
    }

    /// <summary>
    /// Checks the authorization policy.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="resource">The resource name.</param>
    /// <param name="resources">The resources.</param>
    /// <returns>true when authorized, otherwise false</returns>
    public static bool CheckAccess(string action, string resource, params Claim[] resources)
    {
      Check.NotNull(action, nameof(action));
      Check.NotNull(resource, nameof(resource));

      var resourceList = resources.ToList();
      resourceList.Add(new Claim(ResourceType, resource));
      return CheckAccess(action, resourceList.ToArray());
    }

    /// <summary>
    /// Checks the authorization policy.
    /// </summary>
    /// <param name="context">The authorization context.</param>
    /// <returns>true when authorized, otherwise false</returns>
    public static bool CheckAccess(AuthorizationContext context)
    {
      Check.NotNull(context, "context");

      if(EnforceAuthorizationManagerImplementation)
      {
        var authZtype = AuthorizationManager.GetType().FullName;
        if(authZtype.Equals("System.Security.Claims.ClaimsAuthorizationManager"))
        {
          throw new InvalidOperationException("No ClaimsAuthorizationManager implementation configured.");
        }
      }

      return AuthorizationManager.CheckAccess(context);
    }

    public static AuthorizationContext CreateAuthorizationContext(ClaimsPrincipal principal, string action, params string[] resources)
    {
      var actionClaims = new Collection<Claim>() {
        new Claim(ActionType, action)
      };

      var resourceClaims = new Collection<Claim>();

      if(resources != null && resources.Length > 0)
      {
        resources.ToList().ForEach(ar => resourceClaims.Add(new Claim(ResourceType, ar)));
      }

      return new AuthorizationContext(
          principal,
          resourceClaims,
          actionClaims);
    }
  }
}



