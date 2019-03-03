using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Xml;
using Ampl.Identity;
using Ampl.System;

namespace Ampl.Web.Mvc
{
  #region
  /// <summary>
  /// Provides the (database)store-based implementation of the <see cref="ClaimsAuthorizationManager"/>. 
  /// </summary>
  /// <remarks>
  /// <para>To use custom Claims authorization manager in a Web application,
  ///       right after DI container initialization, use the
  ///       <c>FederatedAuthentication.FederationConfigurationCreated</c> event to wire up the manager:
  ///       <em>(example is for Autofac, but other DIs behave similarily)</em></para>
  /// <code>
  ///   FederatedAuthentication.FederationConfigurationCreated += (_s, _e) =>
  ///     {
  ///       _e.FederationConfiguration.IdentityConfiguration.ClaimsAuthorizationManager =
  ///         container.Resolve&lt;ClaimsAuthorizationManager&gt;();
  ///     };
  /// </code>
  /// <para>Alternatively, the Web.config configuration method may be used:</para>
  /// <code>
  ///   &lt;configSections&gt;
  ///     &lt;section name="system.identityModel" type="System.IdentityModel.Configuration.SystemIdentityModelSection, System.IdentityModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=B77A5C561934E089" /&gt;
  ///   &lt;/configSections&gt;
  ///   &lt;system.identityModel&gt;
  ///     &lt;identityConfiguration&gt;
  ///       &lt;claimsAuthorizationManager type="Ampl.Web.Mvc.ApplicationClaimsAuthorizationManager, Ampl.Web.Mvc.Authorization" &gt;
  ///         &lt;authorizationStore type="FULL.TYPE.OF.STORE.IMPLEMENTING.IAuthorizationStore, ASSEMBLY.NAME" /&gt;
  ///       &lt;/claimsAuthorizationManager&gt;
  ///     &lt;/identityConfiguration&gt;
  ///   &lt;/system.identityModel&gt;
  /// </code>
  /// <para>This method requires <see cref="IAuthorizationStore"/> implementation to have parameterless constructor.</para>
  /// </remarks>
  #endregion
  public class ApplicationClaimsAuthorizationManager : ClaimsAuthorizationManager
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationClaimsAuthorizationManager"/> class.
    /// </summary>
    /// <remarks>Web.config/App.config initialization in required to properly set the Authorization Store.
    /// See the class <b>Remarks</b> section.</remarks>
    public ApplicationClaimsAuthorizationManager()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationClaimsAuthorizationManager"/> class.
    /// </summary>
    /// <param name="authStore">The authorization store.</param>
    public ApplicationClaimsAuthorizationManager(IAuthorizationStore authStore) : this()
    {
      _authStore = Check.NotNull(authStore, nameof(authStore));
    }

    private bool MatchesAction(string aclAction, string actualAction)
    {
      //
      // empty ACL Action matches any action, even empty
      //
      if(string.IsNullOrWhiteSpace(aclAction))
      {
        return true;
      }

      //
      // '*' ACL Action mathes any non-empty action
      //
      if(aclAction == "*")
      {
        return !string.IsNullOrWhiteSpace(actualAction);
      }

      //
      // non-empty ACL Action mathches action exactly
      //
      return aclAction == actualAction;
    }

    private bool MatchesResource(string aclResource, string actualResource)
    {
      //
      // empty ACL Resource mathes any resource, even empty
      //
      if(string.IsNullOrWhiteSpace(aclResource))
      {
        return true;
      }

      //
      // '*' ACL Resource matches any non-empty resource
      //
      if(aclResource == "*")
      {
        return !string.IsNullOrWhiteSpace(actualResource);
      }

      //
      // non-empty ACL Resource matches resource exactly
      //
      return aclResource == actualResource;
    }

    private bool MatchesUser(string aclUser, ClaimsPrincipal principal)
    {
      //
      // empty ACL User matches any user name or an empty (anonymous, unauthenticated) user
      //
      if(string.IsNullOrWhiteSpace(aclUser))
      {
        return true;
      }

      //
      // '*' ACL User matches any anthenticated user
      //
      if(aclUser == "*")
      {
        return principal.Identity.IsAuthenticated;
      }

      //
      // non-empty ACL User mathes user's identity name exactly
      //
      return aclUser == principal.Identity.Name;
    }

    private bool MatchesRole(string aclRole, ClaimsPrincipal principal)
    {
      //
      // empty ACL Role matches any role, even empty
      //
      if(string.IsNullOrWhiteSpace(aclRole))
      {
        return true;
      }

      //
      // '*' ACL Role mathes if principal is in any role
      //
      if(aclRole == "*")
      {
        return (principal.Claims ?? Enumerable.Empty<Claim>()).Any(x => x.Type == ClaimTypes.Role);
      }

      //
      // non-empty ACL Role checks against IsInRole
      //
      return principal.IsInRole(aclRole);
    }

    private bool IsAllowed(Claim action, Claim resource, List<IAccessControlList> acls, AuthorizationContext context)
    {
      foreach(var acl in acls)
      {
        bool matches = MatchesAction(acl.Actions, action.Value);
        matches &= MatchesResource(acl.Resources, resource.Value);
        matches &= MatchesUser(acl.Users, context.Principal);
        matches &= MatchesRole(acl.Roles, context.Principal);

        if(matches)
        {
          return acl.Allow;
        }
      }
      return false;
    }

    public override bool CheckAccess(AuthorizationContext context)
    {
      //
      // allow all for Administrator
      //
      if(context.Principal.Identity.Name == "Administrator")
      {
        return true;
      }

      bool allowed = true;

      //
      // the user must be allowed for every action for every resource to be allowed
      // e.g. if actions=Read,Write resources=Balance,Account
      // then 4 ACL must evaluate to Allow:
      //  (Read,Balance) (Read,Accout) (Write,Balance) (Write,Account)
      //
      var acls = _authStore.GetAccessControlLists().OrderBy(acl => acl.Position).ToList();
      foreach(var action in context.Action)
      {
        foreach(var resource in context.Resource)
        {
          allowed &= IsAllowed(action, resource, acls, context);
        }
      }

      return allowed;
    }

    IAuthorizationStore _authStore;

    public override void LoadCustomConfiguration(XmlNodeList nodelist)
    {
      foreach(XmlNode node in nodelist)
      {
        if(node.Name == "authorizationStore")
        {
          foreach(XmlAttribute attribute in node.Attributes)
          {
            if(attribute.Name == "type")
            {
              Type type = (Type)new TypeNameConverter().ConvertFrom(attribute.Value);
              _authStore = (Activator.CreateInstance(type) as IAuthorizationStore);
              if(_authStore == null)
              {
                throw new InvalidOperationException(
                  "The Authorization Store provided for ApplicationClaimsAuthorizationManager " +
                  "does not implement the IAuthorizationStore interface.");
              }
              break;
            }
          }
          break;
        }
      }
      if(_authStore == null)
      {
        throw new InvalidOperationException(
          "No Authorization Store configured in ApplicationClaimsAuthorizationManager configuration.");
      }
    }
  }
}