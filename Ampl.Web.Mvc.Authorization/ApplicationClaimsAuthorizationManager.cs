﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Xml;
using Ampl.Identity.EntityFramework;
using Ampl.System;

namespace Ampl.Web.Mvc
{
  public class ApplicationClaimsAuthorizationManager : ClaimsAuthorizationManager
  {
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
        return principal.Claims.ToEmptyIfNull().Any(x => x.Type == ClaimTypes.Role);
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
      var acls = _db.GetAccessControlLists().OrderBy(acl => acl.Position).ToList();
      foreach(var action in context.Action)
      {
        foreach(var resource in context.Resource)
        {
          allowed &= IsAllowed(action, resource, acls, context);
        }
      }

      return allowed;
    }

    IAuthorizationDbContext _db;

    public override void LoadCustomConfiguration(XmlNodeList nodelist)
    {
      foreach(XmlNode node in nodelist)
      {
        if(node.Name == "applicationDbContext")
        {
          foreach(XmlAttribute attribute in node.Attributes)
          {
            if(attribute.Name == "type")
            {
              Type type = (Type)new TypeNameConverter().ConvertFrom(attribute.Value);
              _db = (Activator.CreateInstance(type) as IAuthorizationDbContext);
              if(_db == null)
              {
                throw new InvalidOperationException(
                  "ApplicationDbContext provided for ApplicationClaimsAuthorizationManager " +
                  "does not implement the IAuthorizationDbContext interface.");
              }
              break;
            }
          }
          break;
        }
      }
      if(_db == null)
      {
        throw new InvalidOperationException(
          "No ApplicationDbContext configured in ApplicationClaimsAuthorizationManager configuration.");
      }
    }
  }
}