using System;
using System.Collections.Generic;
using System.Web;
using Ampl.System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Ampl.Web.Mvc.SecondaryLogon
{
  public class SecondaryLogonManager<TUser, TKey> : ISecondaryLogonManager<TKey>
    where TUser : class, IUser<TKey>
    where TKey : IEquatable<TKey>
  {
    private readonly HttpContextBase _context;
    private readonly UserManager<TUser, TKey> _userManager;
    private readonly IAuthenticationManager _authenticationManager;
    private readonly SignInManager<TUser, TKey> _signInManager;
    private readonly object _locker = new object();


    public SecondaryLogonManager(HttpContextBase context,
                                 UserManager<TUser, TKey> userManager,
                                 IAuthenticationManager authenticationManager,
                                 SignInManager<TUser, TKey> signInManager)
    {
      _context = Check.NotNull(context, nameof(context));
      _userManager = Check.NotNull(userManager, nameof(userManager));
      _authenticationManager = Check.NotNull(authenticationManager, nameof(authenticationManager));
      _signInManager = Check.NotNull(signInManager, nameof(signInManager));
    }

    private Stack<Tuple<TKey, string>> GetLogonStack()
    {
      Stack<Tuple<TKey, string>> result = null;
      lock(_locker)
      {
        string sessionKey = this.GetType().FullName;
        result = (_context.Session[sessionKey] as Stack<Tuple<TKey, string>>);
        if(result == null)
        {
          result = new Stack<Tuple<TKey, string>>();
          _context.Session[sessionKey] = result;
        }
      }
      return result;
    }

    public void LogonAs(TKey userID)
    {
      LogonAs(userID, null);
    }

    public void LogonAs(TKey userID, string previousUrl)
    {
      var user = _userManager.FindById(Check.NotNull(userID, nameof(userID)));
      if(user == null)
      {
        return;
      }
      var currentUser = _userManager.FindByName(_authenticationManager.User.Identity.Name);
      if(currentUser != null)
      {
        GetLogonStack().Push(new Tuple<TKey, string>(currentUser.Id, previousUrl));
      }

      _authenticationManager.SignOut();
      _signInManager.SignIn(user, false, false);
    }

    public ISecondaryLogonResult LogonAsPreviousUser()
    {
      var stack = GetLogonStack();
      if(stack.Count == 0)
      {
        return new SecondaryLogonResult(false, null);
      }

      var previousLogin = stack.Pop();
      var user = _userManager.FindById(previousLogin.Item1);
      if(user == null)
      {
        return new SecondaryLogonResult(false, null);
      }

      _authenticationManager.SignOut();
      _signInManager.SignIn(user, false, false);
      return new SecondaryLogonResult(true, previousLogin.Item2);
    }

    private class SecondaryLogonResult : ISecondaryLogonResult
    {
      public bool Success { get; private set; }
      public string PreviousUrl { get; private set; }
      public SecondaryLogonResult(bool success, string previousUrl)
      {
        Success = success;
        PreviousUrl = previousUrl;
      }
    }

  }
}